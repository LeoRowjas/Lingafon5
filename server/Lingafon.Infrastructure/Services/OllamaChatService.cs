using System.Net.Http.Json;
using System.Text.Json;
using Lingafon.Core.Interfaces.Services;

namespace Lingafon.Infrastructure.Services;

public class OllamaChatService : IAiChatService
{
    private readonly HttpClient _httpClient;
    private readonly string _model;

    public OllamaChatService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _model = "mistral";
    }

    public async Task<string> GetReplyAsync(string systemPrompt, List<(string role, string content)> messages)
    {
        var requestBody = new
        {
            model = _model,
            messages = BuildMessages(systemPrompt, messages),
            stream = false
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "http://ollama:11434/api/chat");
        request.Content = JsonContent.Create(requestBody);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("message").GetProperty("content").GetString() ?? string.Empty;
    }

    private static List<object> BuildMessages(string systemPrompt, List<(string role, string content)> messages)
    {
        var result = new List<object>
        {
            new { role = "system", content = systemPrompt }
        };

        foreach (var msg in messages)
        {
            result.Add(new { role = msg.role, content = msg.content });
        }

        return result;
    }
}

