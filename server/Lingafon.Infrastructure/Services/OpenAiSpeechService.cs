using System.Text;
using Lingafon.Core.Interfaces.Services;
using Lingafon.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Whisper.net;


namespace Lingafon.Infrastructure.Services;

public class OpenAiSpeechService : IAiSpeechService
{
    private readonly IFileStorageService _fileStorage;
    private readonly S3Settings _s3Settings;
    private readonly string _modelPath;

    public OpenAiSpeechService(
        IFileStorageService fileStorage,
        IOptions<S3Settings> s3Settings,
        string modelPath)
    {
        _fileStorage = fileStorage;
        _s3Settings = s3Settings.Value;
        _modelPath = modelPath;
    }

    public async Task<string?> GetTextFromSpeechAsync(string audioPath)
    {
        try
        {
            Console.WriteLine($"[OpenAiSpeechService] Processing audio file: {audioPath}");

            // Validate audio format
            if (!AudioConversionHelper.IsSupportedAudioFormat(audioPath))
            {
                Console.WriteLine($"[OpenAiSpeechService] Unsupported audio format: {AudioConversionHelper.GetAudioFormatName(audioPath)}");
                return null;
            }
            
            // Convert audio to 16KHz WAV format required by Whisper
            var processedAudioPath = AudioConversionHelper.ConvertToWhisperFormat(audioPath);
            Console.WriteLine($"[OpenAiSpeechService] Converted audio to: {processedAudioPath}");
            
            using var factory = WhisperFactory.FromPath(_modelPath);
            await using var processor = factory.CreateBuilder().WithLanguage("auto").Build();

            await using var audioStream = File.OpenRead(processedAudioPath);
            var sb = new StringBuilder();
            await foreach (var segment in processor.ProcessAsync(audioStream))
            {
                sb.Append(segment.Text);
            }
            
            var transcription = sb.ToString();
            Console.WriteLine($"[OpenAiSpeechService] Transcription result: {transcription}");
            
            // Clean up converted file if it's different from original
            if (processedAudioPath != audioPath && File.Exists(processedAudioPath))
            {
                try { File.Delete(processedAudioPath); }
                catch { }
            }
            
            return transcription;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[OpenAiSpeechService] Error during transcription: {ex.GetType().Name} - {ex.Message}");
            return null;
        }
    }


    public async Task<string?> GetSpeechFromTextAsync(string text)
    {
        throw new NotImplementedException();
    }
}