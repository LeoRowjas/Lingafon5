using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Lingafon.Core.Interfaces.Services;
using Lingafon.Infrastructure.Settings;
using Microsoft.Extensions.Options;
using Whisper.net;

namespace Lingafon.Infrastructure.Services;

public class CoquiSpeechService : IAiSpeechService
{
    private readonly IFileStorageService _fileStorage;
    private readonly S3Settings _s3Settings;
    private readonly string _whisperModelPath;
    private readonly HttpClient _httpClient;
    private readonly string _kokoroTtsUrl;

    public CoquiSpeechService(
        IFileStorageService fileStorage,
        IOptions<S3Settings> s3Settings,
        string modelPath,
        HttpClient httpClient)
    {
        _fileStorage = fileStorage;
        _s3Settings = s3Settings.Value;
        _whisperModelPath = modelPath;
        _httpClient = httpClient;
        _kokoroTtsUrl = Environment.GetEnvironmentVariable("KOKORO_TTS_URL") ?? "http://localhost:8080";
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
        try
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            Console.WriteLine($"[CoquiSpeechService] Starting TTS for text: {text}");
            Console.WriteLine($"[CoquiSpeechService] Using Kokoro TTS service at: {_kokoroTtsUrl}");

            // Use default voice (af_bella as per Kokoro TTS readme)
            var voice = "af_bella";
            Console.WriteLine($"[CoquiSpeechService] Using voice: {voice}");

            // Create JSON payload for Kokoro TTS API
            var requestBody = new { text = text, voice = voice };
            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            Console.WriteLine($"[CoquiSpeechService] Calling Kokoro TTS API POST /tts with JSON...");

            var response = await _httpClient.PostAsync($"{_kokoroTtsUrl}/tts", content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"[CoquiSpeechService] Kokoro TTS API returned error: {response.StatusCode} - {response.ReasonPhrase}");
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[CoquiSpeechService] Error response: {errorContent}");
                return null;
            }

            var audioBytes = await response.Content.ReadAsByteArrayAsync();
            Console.WriteLine($"[CoquiSpeechService] Synthesized {audioBytes.Length} bytes");

            // Save audio to temporary file
            var filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.wav");
            try
            {
                await File.WriteAllBytesAsync(filePath, audioBytes);
                Console.WriteLine($"[CoquiSpeechService] Synthesis saved to: {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CoquiSpeechService] Failed to save audio file: {ex.GetType().Name} - {ex.Message}");
                return null;
            }

            // Upload to storage
            try
            {
                await using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                var fileName = Path.GetFileName(filePath);
                Console.WriteLine($"[CoquiSpeechService] Uploading synthesized file {fileName} to bucket {_s3Settings.BucketNameAudio}");
                var uploadedUrl = await _fileStorage.UploadFileAsync(fs, fileName, "audio/wav", _s3Settings.BucketNameAudio);
                Console.WriteLine($"[CoquiSpeechService] Uploaded synthesized audio to: {uploadedUrl}");

                try { File.Delete(filePath); }
                catch (Exception ex)
                {
                    Console.WriteLine($"[CoquiSpeechService] Failed to delete temp file after upload: {ex.GetType().Name} - {ex.Message}");
                }

                return uploadedUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CoquiSpeechService] Failed to upload synthesized audio: {ex.GetType().Name} - {ex.Message}");
                try { File.Delete(filePath); }
                catch (Exception dex)
                {
                    Console.WriteLine($"[CoquiSpeechService] Failed to delete temp file after upload failure: {dex.GetType().Name} - {dex.Message}");
                }
                return null;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[CoquiSpeechService] Error during TTS: {ex.GetType().Name} - {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            return null;
        }
    }
}

