namespace Lingafon.Core.Interfaces.Services;

public interface IAiSpeechService
{ 
    Task<string?> GetTextFromSpeechAsync(string audioUrl);
    Task<string?> GetSpeechFromTextAsync(string text);
}