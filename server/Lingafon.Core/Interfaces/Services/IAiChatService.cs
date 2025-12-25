using Lingafon.Core.Enums;

namespace Lingafon.Core.Interfaces.Services;

public interface IAiChatService
{
    Task<string> GetReplyAsync(string systemPrompt, List<(string role, string content)> messages);
}