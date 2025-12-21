using NAudio.Wave;


//MARK: FULL GPT CODE
namespace Lingafon.Infrastructure.Services;

/// <summary>
/// Helper class for converting audio files to 16KHz WAV format required by Whisper.net
/// Supports MP3, M4A, WAV, and other formats supported by NAudio
/// Uses FFmpeg for M4A/AAC files on Linux to avoid Windows Media Foundation dependency
/// </summary>
public static class AudioConversionHelper
{
    private const int WhisperSampleRate = 16000;
    private const int WhisperBitDepth = 16;

    /// <summary>
    /// Converts any audio file to 16KHz WAV format required by Whisper
    /// </summary>
    /// <param name="inputPath">Path to input audio file (MP3, M4A, WAV, etc.)</param>
    /// <returns>Path to converted 16KHz WAV file, or original path if already 16KHz</returns>
    public static string ConvertToWhisperFormat(string inputPath)
    {
        try
        {
            Console.WriteLine($"[AudioConversionHelper] Processing audio file: {inputPath}");
            Console.WriteLine($"[AudioConversionHelper] File extension: {Path.GetExtension(inputPath)}");

            var extension = Path.GetExtension(inputPath).ToLower();
            
            // For M4A/AAC files, use FFmpeg on Linux to avoid Windows Media Foundation issues
            if ((extension == ".m4a" || extension == ".aac") && IsLinux())
            {
                Console.WriteLine($"[AudioConversionHelper] M4A/AAC detected on Linux, using FFmpeg...");
                return ConvertAudioWithFFmpeg(inputPath);
            }

            // For other formats, use NAudio
            return ConvertAudioWithNAudio(inputPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AudioConversionHelper] Error converting audio: {ex.GetType().Name} - {ex.Message}");
            throw;
        }
    }

    /// <summary>
    /// Converts audio using NAudio (for Windows/macOS)
    /// </summary>
    private static string ConvertAudioWithNAudio(string inputPath)
    {
        using var reader = new AudioFileReader(inputPath);
        var sourceFormat = reader.WaveFormat;
        
        Console.WriteLine($"[AudioConversionHelper] Input audio - Sample rate: {sourceFormat.SampleRate}Hz, Channels: {sourceFormat.Channels}, Bit depth: {sourceFormat.BitsPerSample}");

        // If already 16KHz WAV, return as-is
        if (sourceFormat.SampleRate == WhisperSampleRate && inputPath.EndsWith(".wav", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine($"[AudioConversionHelper] Audio already in Whisper format (16KHz WAV), skipping conversion");
            return inputPath;
        }

        // Create output path with .16khz.wav extension
        var outputPath = Path.ChangeExtension(inputPath, ".16khz.wav");
        var targetFormat = new WaveFormat(WhisperSampleRate, WhisperBitDepth, sourceFormat.Channels);

        Console.WriteLine($"[AudioConversionHelper] Converting to: {outputPath}");
        Console.WriteLine($"[AudioConversionHelper] Target format - Sample rate: {targetFormat.SampleRate}Hz, Channels: {targetFormat.Channels}, Bit depth: {targetFormat.BitsPerSample}");

        // Resample audio to 16KHz
        ResampleAudio(reader, outputPath, targetFormat);

        Console.WriteLine($"[AudioConversionHelper] Audio successfully converted to 16KHz WAV: {outputPath}");
        return outputPath;
    }

    /// <summary>
    /// Converts audio using FFmpeg (for M4A/AAC on Linux)
    /// </summary>
    private static string ConvertAudioWithFFmpeg(string inputPath)
    {
        var outputPath = Path.ChangeExtension(inputPath, ".16khz.wav");
        
        try
        {
            Console.WriteLine($"[AudioConversionHelper] Converting with FFmpeg: {inputPath} -> {outputPath}");
            
            // FFmpeg command to convert to 16KHz mono WAV
            var ffmpegArgs = $"-i \"{inputPath}\" -acodec pcm_s16le -ar 16000 -ac 1 \"{outputPath}\" -y";
            
            var processInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = "ffmpeg",
                Arguments = ffmpegArgs,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = System.Diagnostics.Process.Start(processInfo);
            
            if (process == null)
            {
                throw new InvalidOperationException("Failed to start FFmpeg process");
            }

            var output = process.StandardOutput.ReadToEnd();
            var error = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new InvalidOperationException($"FFmpeg conversion failed: {error}");
            }

            Console.WriteLine($"[AudioConversionHelper] FFmpeg conversion successful: {outputPath}");
            return outputPath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AudioConversionHelper] FFmpeg conversion error: {ex.Message}");
            Console.WriteLine($"[AudioConversionHelper] Ensure FFmpeg is installed: apt-get install -y ffmpeg");
            throw;
        }
    }

    /// <summary>
    /// Resamples audio from source format to target format using linear interpolation
    /// </summary>
    private static void ResampleAudio(AudioFileReader reader, string outputPath, WaveFormat targetFormat)
    {
        float[] buffer = new float[reader.WaveFormat.SampleRate]; // 1 second buffer
        List<float> resampledData = new();

        int samplesRead;
        while ((samplesRead = reader.Read(buffer, 0, buffer.Length)) > 0)
        {
            // Calculate resampling ratio
            float ratio = reader.WaveFormat.SampleRate / (float)targetFormat.SampleRate;
            int resampledLength = (int)(samplesRead / ratio);

            // Linear interpolation resampling
            for (int i = 0; i < resampledLength; i++)
            {
                float srcPos = i * ratio;
                int srcIndex = (int)srcPos;
                float fraction = srcPos - srcIndex;

                if (srcIndex + 1 < samplesRead)
                {
                    // Interpolate between two samples
                    float sample = buffer[srcIndex] * (1 - fraction) + buffer[srcIndex + 1] * fraction;
                    resampledData.Add(sample);
                }
                else if (srcIndex < samplesRead)
                {
                    // Last sample, no interpolation needed
                    resampledData.Add(buffer[srcIndex]);
                }
            }
        }

        // Write resampled audio to WAV file
        using (var writer = new WaveFileWriter(outputPath, targetFormat))
        {
            writer.WriteSamples(resampledData.ToArray(), 0, resampledData.Count);
        }

        Console.WriteLine($"[AudioConversionHelper] Wrote {resampledData.Count} samples to {outputPath}");
    }

    /// <summary>
    /// Checks if running on Linux
    /// </summary>
    private static bool IsLinux()
    {
        return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
    }

    /// <summary>
    /// Checks if a file is in a supported audio format
    /// </summary>
    public static bool IsSupportedAudioFormat(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLower();
        return extension switch
        {
            ".wav" => true,
            ".mp3" => true,
            ".m4a" => true,
            ".aac" => true,
            ".flac" => true,
            ".wma" => true,
            ".ogg" => true,
            ".webm" => true,
            _ => false
        };
    }

    /// <summary>
    /// Gets a friendly name for the audio format
    /// </summary>
    public static string GetAudioFormatName(string filePath)
    {
        var extension = Path.GetExtension(filePath).ToLower();
        return extension switch
        {
            ".wav" => "WAV",
            ".mp3" => "MP3",
            ".m4a" => "M4A (AAC)",
            ".aac" => "AAC",
            ".flac" => "FLAC",
            ".wma" => "WMA",
            ".ogg" => "OGG Vorbis",
            ".webm" => "WebM (Opus)",
            _ => "Unknown"
        };
    }
}

