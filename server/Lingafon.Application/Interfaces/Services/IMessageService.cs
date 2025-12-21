using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.DTOs;

namespace Lingafon.Application.Interfaces.Services;

public interface IMessageService : IService<MessageReadDto, MessageCreateDto, MessageCreateDto>
{
    Task<IEnumerable<MessageReadDto>> GetByDialogAsync(Guid dialogId);
    Task<string> CreateVoiceAsync(VoiceMessageCreateDto dto);
    Task<AiReplyResponse> GetAiReplyAsync(Guid userId, AiReplyRequest request);
}
