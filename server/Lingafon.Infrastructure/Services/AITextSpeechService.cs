using System.Text;
using System.Text.Json;
using Lingafon.Core.Interfaces.Services;
using Lingafon.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Whisper.net;

namespace Lingafon.Infrastructure.Services;

public class AiTextSpeechService : IAiSpeechService
{
    private readonly IFileStorageService _fileService;
    private readonly S3Settings _s3Settings;
    private readonly string _whisperModelPath;
    private readonly HttpClient _httpClient;

    public AiTextSpeechService(
        IFileStorageService fileStorage,
        IOptions<S3Settings> s3Settings,
        string modelPath,
        HttpClient httpClient)
    {
        _fileService = fileStorage;
        _s3Settings = s3Settings.Value;
        _whisperModelPath = modelPath;
        _httpClient = httpClient;
    }

    public async Task<string?> GetTextFromSpeechAsync(string audioPath)
    {
        try
        {
            Console.WriteLine($"[CoquiSpeechService] Processing audio file: {audioPath}");

            if (string.IsNullOrEmpty(_whisperModelPath) || !File.Exists(_whisperModelPath))
            {
                Console.WriteLine($"[CoquiSpeechService] Whisper model not available at {_whisperModelPath}");
                return null;
            }

            if (!AudioConversionHelper.IsSupportedAudioFormat(audioPath))
            {
                Console.WriteLine($"[CoquiSpeechService] Unsupported audio format: {AudioConversionHelper.GetAudioFormatName(audioPath)}");
                return null;
            }

            var processedAudioPath = AudioConversionHelper.ConvertToWhisperFormat(audioPath);
            Console.WriteLine($"[CoquiSpeechService] Converted audio to: {processedAudioPath}");

            using var factory = WhisperFactory.FromPath(_whisperModelPath);
            using var processor = factory.CreateBuilder()
                .WithLanguage("en")
                .Build();

            await using var audioStream = File.OpenRead(processedAudioPath);
            var sb = new StringBuilder();
            await foreach (var segment in processor.ProcessAsync(audioStream))
            {
                sb.Append(segment.Text);
            }

            var transcription = sb.ToString();
            Console.WriteLine($"[CoquiSpeechService] Transcription result: {transcription}");

            try { File.Delete(processedAudioPath); }
            catch (Exception ex)
            {
                Console.WriteLine($"[CoquiSpeechService] Failed to delete processed audio temp file: {ex.GetType().Name} - {ex.Message}");
            }

            return transcription;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[CoquiSpeechService] Error during transcription: {ex.GetType().Name} - {ex.Message}");
            return null;
        }
    }

    public async Task<string?> GetSpeechFromTextAsync(string text)
    {
        var splitText = text
            .Split(new[] {' ', '\t', '\n'})
            .Select((w, i) => new {w, i})
            .GroupBy(x => x.i / 10)
            .Select(g => g.Select(x => x.w).ToList())
            .ToList();
        
        var requestBody = new
        {
            model = "kokoro",
            input = text,
            voice = "af_heart",
            response_format = "wav",
            speed = 1.0
        };

        var json = JsonSerializer.Serialize(requestBody);

        var content = new StringContent(
            json,
            Encoding.UTF8,
            "application/json"
        );

        Console.WriteLine($"[CoquiSpeechService] Processing audio file");
        var baseAddress = Environment.GetEnvironmentVariable("KOKORO_BASE_URL") ?? "http://kokoro:8880";
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, $"{baseAddress}/v1/audio/speech");
        httpRequest.Content = content;
        var response = await _httpClient.SendAsync(httpRequest);

        response.EnsureSuccessStatusCode();

        Console.WriteLine("начало получения байтов");
        // получаем WAV как байты
        var audioBytes = await response.Content.ReadAsByteArrayAsync();
        
        Console.WriteLine("сохраняем байты");
        // сохраняем в файл
        var fileName = $"{Guid.NewGuid()}.wav";
        var filePath = Path.Combine(Path.GetTempPath(), fileName);
        await File.WriteAllBytesAsync(filePath, audioBytes);
        await using var stream = new FileStream(filePath, FileMode.Open);
        
        var audioUrl = await _fileService.UploadFileAsync(
            stream,
            fileName,
            "multipart/form-data",
            _s3Settings.BucketNameAudio);
        
        try { File.Delete(filePath); }
        catch { }

        return audioUrl;
    }
}

