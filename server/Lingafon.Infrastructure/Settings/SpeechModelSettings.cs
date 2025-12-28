namespace Lingafon.Infrastructure.Settings;

public class SpeechModelSettings
{
    // Path to Whisper model used for speech-to-text (ggml model or path to file)
    public string WhisperModelPath { get; set; } = string.Empty;

    // Directory where TTS models (Coqui / Kokoro) are stored (host mounted into containers if needed)
    public string TtsModelsDirectory { get; set; } = string.Empty;
}

