using System.Net.Http.Json;
using System.Text.Json;
using Lingafon.Core.Interfaces.Repositories;
using Lingafon.Core.Interfaces.Services;

namespace Lingafon.Infrastructure.Services;

public class OllamaChatService : IAiChatService
{
    private readonly IMessageRepository _messageRepository;
    private readonly HttpClient _httpClient;
    private readonly string _model;

    public OllamaChatService(HttpClient httpClient, IMessageRepository messageRepository)
    {
        _httpClient = httpClient;
        _messageRepository = messageRepository;
        _model = "phi:latest";
    }

    public async Task<string> GetReplyAsync(string systemPrompt, List<(string role, string content)> messages)
    {
        // Build prompt with system prompt and conversation history
        var prompt = BuildPrompt(systemPrompt, messages);

        var requestBody = new
        {
            model = _model,
            prompt,
            stream = false
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "http://ollama:11434/api/generate");
        request.Content = JsonContent.Create(requestBody);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("response").GetString() ?? string.Empty;
    }

    private static string BuildPrompt(string systemPrompt, List<(string role, string content)> messages)
    {
        var sb = new System.Text.StringBuilder();
        
        // Add system prompt
        sb.AppendLine($"System: {systemPrompt}");
        sb.AppendLine();

        // Add conversation history
        foreach (var msg in messages)
        {
            var roleLabel = msg.role == "user" ? "User" : "Assistant";
            sb.AppendLine($"{roleLabel}: {msg.content}");
        }

        sb.AppendLine("Assistant:");
        return sb.ToString();
    }
}

