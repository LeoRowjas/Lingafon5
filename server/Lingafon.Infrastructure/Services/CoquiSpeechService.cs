using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
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

    private readonly string? _openTtsUrl;

    public OpenAiSpeechService(
        IFileStorageService fileStorage,
        IOptions<S3Settings> s3Settings,
        string modelPath)
    {
        _fileStorage = fileStorage;
        _s3Settings = s3Settings.Value;
        _modelPath = modelPath;
        _openTtsUrl = Environment.GetEnvironmentVariable("OPEN_TTS_URL");
    }

    public async Task<string?> GetTextFromSpeechAsync(string audioPath)
    {
        try
        {
            Console.WriteLine($"[OpenAiSpeechService] Processing audio file: {audioPath}");

            if (string.IsNullOrEmpty(_modelPath) || !File.Exists(_modelPath))
            {
                Console.WriteLine($"[OpenAiSpeechService] Whisper model not available at {_modelPath}");
                return null;
            }

            if (!AudioConversionHelper.IsSupportedAudioFormat(audioPath))
            {
                Console.WriteLine($"[OpenAiSpeechService] Unsupported audio format: {AudioConversionHelper.GetAudioFormatName(audioPath)}");
                return null;
            }

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
        try
        {
            if (string.IsNullOrWhiteSpace(text))
                return null;

            Console.WriteLine($"[OpenAiSpeechService] Converting text to speech: {text.Substring(0, Math.Min(50, text.Length))}...");

            var audioFileName = $"ai_reply_{Guid.NewGuid()}.wav";
            var audioPath = Path.Combine(Path.GetTempPath(), audioFileName);

            // If OPEN_TTS_URL configured, prefer remote OpenTTS service
            if (!string.IsNullOrEmpty(_openTtsUrl))
            {
                try
                {
                    Console.WriteLine($"[OpenAiSpeechService] Using OpenTTS at {_openTtsUrl}");
                    using var http = new HttpClient { BaseAddress = new Uri(_openTtsUrl) };

                    var payload = new
                    {
                        text = text,
                        voice = "alloy",
                        format = "wav"
                    };

                    var resp = await http.PostAsJsonAsync("api/tts", payload);
                    if (!resp.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"[OpenAiSpeechService] OpenTTS returned {(int)resp.StatusCode} - {resp.ReasonPhrase}");
                    }
                    else
                    {
                        var bytes = await resp.Content.ReadAsByteArrayAsync();
                        await File.WriteAllBytesAsync(audioPath, bytes);
                        Console.WriteLine($"[OpenAiSpeechService] Received wav from OpenTTS, saved to {audioPath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[OpenAiSpeechService] OpenTTS call failed: {ex.Message}");
                }
            }

            // If file not created by OpenTTS, fallback to local TTS commands
            if (!File.Exists(audioPath))
            {
                Console.WriteLine("[OpenAiSpeechService] Falling back to local TTS command");

                // Try espeak for Linux (Docker), fallback to say for macOS
                var ttsCommand = GetTTSCommand(out var arguments);

                if (string.IsNullOrEmpty(ttsCommand))
                {
                    Console.WriteLine($"[OpenAiSpeechService] No TTS command available (espeak or say not found)");
                    return null;
                }

                var process = new ProcessStartInfo
                {
                    FileName = ttsCommand,
                    Arguments = string.Format(arguments, text, audioPath),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                Console.WriteLine($"[OpenAiSpeechService] Executing: {ttsCommand} {process.Arguments}");

                using (var proc = Process.Start(process))
                {
                    if (proc != null)
                    {
                        await proc.WaitForExitAsync();
                        var error = await proc.StandardError.ReadToEndAsync();

                        if (proc.ExitCode != 0)
                        {
                            Console.WriteLine($"[OpenAiSpeechService] TTS command error (exit code {proc.ExitCode}): {error}");
                            return null;
                        }
                    }
                    else
                    {
                        Console.WriteLine($"[OpenAiSpeechService] Failed to start TTS process");
                        return null;
                    }
                }

                if (!File.Exists(audioPath))
                {
                    Console.WriteLine($"[OpenAiSpeechService] Failed to generate audio file at {audioPath}");
                    return null;
                }

                Console.WriteLine($"[OpenAiSpeechService] Audio file generated successfully");
            }

            await using var audioStream = new FileStream(audioPath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var audioUrl = await _fileStorage.UploadFileAsync(
                audioStream,
                audioFileName,
                "audio/wav",
                _s3Settings.BucketNameAudio
            );

            Console.WriteLine($"[OpenAiSpeechService] Audio uploaded to S3: {audioUrl}");

            try { File.Delete(audioPath); }
            catch { }

            return audioUrl;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[OpenAiSpeechService] Error converting text to speech: {ex.GetType().Name} - {ex.Message}");
            Console.WriteLine($"[OpenAiSpeechService] Stack trace: {ex.StackTrace}");
            return null;
        }
    }

    private string? GetTTSCommand(out string arguments)
    {
        // Check if espeak is available (Linux/Docker)
        if (CommandExists("espeak"))
        {
            arguments = "-w \"{1}\" \"{0}\"";
            return "espeak";
        }

        // Fallback to say (macOS)
        if (CommandExists("say"))
        {
            arguments = "-o \"{1}\" -f \"{0}\"";
            return "say";
        }

        arguments = string.Empty;
        return null;
    }

    private bool CommandExists(string command)
    {
        try
        {
            var processInfo = new ProcessStartInfo
            {
                FileName = "which",
                Arguments = command,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var process = Process.Start(processInfo))
            {
                if (process != null)
                {
                    process.WaitForExit();
                    return process.ExitCode == 0;
                }
            }
        }
        catch
        {
        }

        return false;
    }
}
