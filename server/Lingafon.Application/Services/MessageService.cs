using AutoMapper;
using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.DTOs;
using Lingafon.Application.Interfaces.Services;
using Lingafon.Core.Entities;
using Lingafon.Core.Interfaces.Repositories;
using Lingafon.Core.Interfaces.Services;

namespace Lingafon.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repository;
    private readonly IMapper _mapper;
    private readonly IAiSpeechService _speechService;
    private readonly IAiChatService _aiChatService;
    private readonly IFileStorageService _fileService;
    private readonly StorageSettings _storageSettings;

    public MessageService(IMessageRepository repository,
        IMapper mapper,
        IAiSpeechService speechService,
        IAiChatService aiChatService,
        IFileStorageService fileService,
        StorageSettings storageSettings)
    {
        _repository = repository;
        _mapper = mapper;
        _speechService = speechService;
        _aiChatService = aiChatService;
        _fileService = fileService;
        _storageSettings = storageSettings;
    }

    public async Task<MessageReadDto?> GetByIdAsync(Guid id)
    {
        var message = await _repository.GetByIdAsync(id);
        return message == null ? null : _mapper.Map<MessageReadDto>(message);
    }

    public async Task<IEnumerable<MessageReadDto>> GetAllAsync()
    {
        var messages = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<MessageReadDto>>(messages);
    }

    public async Task<MessageReadDto> CreateAsync(MessageCreateDto dto)
    {
        var message = _mapper.Map<Message>(dto);
        await _repository.AddAsync(message);
        return _mapper.Map<MessageReadDto>(message);
    }

    public async Task<string> CreateVoiceAsync(VoiceMessageCreateDto dto)
    {
        var tempPath = Path.Combine(Path.GetTempPath(), $"lingafon_{Guid.NewGuid()}{Path.GetExtension(dto.FileName)}");
        await using (var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
        {
            await dto.AudioStream.CopyToAsync(fs);
        }
        
        string audioUrl;
        await using (var uploadStream = new FileStream(tempPath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            audioUrl = await _fileService.UploadFileAsync(
                uploadStream,
                dto.FileName,
                dto.ContentType,
                _storageSettings.BucketNameAudio
            );
        }
        
        var transcription = await _speechService.GetTextFromSpeechAsync(tempPath) ?? string.Empty;

        var message = new Message
        {
            Content = transcription,
            SentAt = DateTime.UtcNow,
            IsFromAi = false,
            AudioUrl = audioUrl,
            DialogId = dto.DialogId,
            SenderId = dto.SenderId
        };

        await _repository.AddAsync(message);

        try { File.Delete(tempPath); }
        catch { }

        return transcription;
    }

    public async Task UpdateAsync(MessageCreateDto dto)
    {
        var message = _mapper.Map<Message>(dto);
        await _repository.UpdateAsync(message);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<MessageReadDto>> GetByDialogAsync(Guid dialogId)
    {
        var messages = await _repository.GetByDialogIdAsync(dialogId);
        return _mapper.Map<IEnumerable<MessageReadDto>>(messages);
    }

    public async Task<AiReplyResponse> GetAiReplyAsync(Guid userId, AiReplyRequest request)
    {
        try
        {
            var messages = await _repository.GetByDialogIdAsync(request.DialogId);
            var messageList = messages.ToList();

            if (messageList.Count == 0)
            {
                return new AiReplyResponse
                {
                    Success = false,
                    ErrorMessage = "No messages in dialog to process",
                    DialogId = request.DialogId,
                    ReplyAt = DateTime.UtcNow
                };
            }

            var lastUserMessage = messageList
                .Where(m => !m.IsFromAi)
                .OrderByDescending(m => m.SentAt)
                .FirstOrDefault();

            if (lastUserMessage == null)
            {
                return new AiReplyResponse
                {
                    Success = false,
                    ErrorMessage = "No user messages found in dialog",
                    DialogId = request.DialogId,
                    ReplyAt = DateTime.UtcNow
                };
            }

            var conversationHistory = new List<(string role, string content)>();
            
            if (request.IncludeHistory && messageList.Count > 1)
            {
                var historyMessages = messageList
                    .OrderByDescending(m => m.SentAt)
                    .Take(request.HistoryLimit)
                    .OrderBy(m => m.SentAt)
                    .ToList();

                foreach (var msg in historyMessages)
                {
                    var role = msg.IsFromAi ? "assistant" : "user";
                    conversationHistory.Add((role, msg.Content));
                }
            }
            else
            {
                conversationHistory.Add(("user", lastUserMessage.Content));
            }

            var systemPrompt = GetTeacherSystemPrompt();
            var aiReply = await _aiChatService.GetReplyAsync(systemPrompt, conversationHistory);

            if (string.IsNullOrEmpty(aiReply))
            {
                return new AiReplyResponse
                {
                    Success = false,
                    ErrorMessage = "AI service returned an empty response",
                    DialogId = request.DialogId,
                    ReplyAt = DateTime.UtcNow
                };
            }

            Console.WriteLine($"[MessageService] Got AI reply: {aiReply.Substring(0, Math.Min(50, aiReply.Length))}...");
            
            var audioUrl = await _speechService.GetSpeechFromTextAsync(aiReply);
            
            if (string.IsNullOrEmpty(audioUrl))
            {
                Console.WriteLine($"[MessageService] Warning: Audio URL is empty after TTS conversion");
            }
            else
            {
                Console.WriteLine($"[MessageService] Audio URL generated: {audioUrl}");
            }

            var aiMessage = new Message
            {
                Content = aiReply,
                SentAt = DateTime.UtcNow,
                IsFromAi = true,
                AudioUrl = audioUrl ?? string.Empty,
                DialogId = request.DialogId,
                SenderId = userId
            };

            await _repository.AddAsync(aiMessage);

            return new AiReplyResponse
            {
                Reply = aiReply,
                AudioUrl = audioUrl,
                DialogId = request.DialogId,
                ReplyAt = DateTime.UtcNow,
                Success = true
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[MessageService] Error in GetAiReplyAsync: {ex.Message}");
            Console.WriteLine($"[MessageService] Stack trace: {ex.StackTrace}");
            return new AiReplyResponse
            {
                Success = false,
                ErrorMessage = ex.Message,
                DialogId = request.DialogId,
                ReplyAt = DateTime.UtcNow
            };
        }
    }

    private string GetTeacherSystemPrompt()
    {
        return @"You are an English language teacher. Your role is to:
1. Correct any grammar, spelling, or punctuation mistakes the student makes
2. Provide constructive feedback on their writing
3. Explain corrections clearly
4. Encourage the student to learn
5. Continue the conversation naturally while maintaining the teaching role
6. Be patient and supportive

When correcting mistakes, explain why the correction is needed and provide the correct form.";
    }
}
