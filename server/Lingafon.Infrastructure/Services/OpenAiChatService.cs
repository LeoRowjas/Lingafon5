using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Lingafon.Core.Enums;
using Lingafon.Core.Interfaces.Services;
using Lingafon.Infrastructure.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Lingafon.Infrastructure.Services;

public class OpenAiChatService : IAiChatService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _model;

    public OpenAiChatService(HttpClient httpClient, IOptions<OpenAiSettings> options)
    {
        _httpClient = httpClient;
        _apiKey = options.Value.ApiKey;
        _model = options.Value.Model ?? "gpt-3.5-turbo";
    }

    public async Task<string> GetReplyAsync(string systemPrompt,
        List<(string role, string content)> messages)
    {
        var requestBody = new
        {
            model = "gpt-3.5-turbo",
            messages = BuildMessages(systemPrompt, messages)
        };

        var request = new HttpRequestMessage(
            HttpMethod.Post, 
            "https://api.openai.com/v1/chat/completions");

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", _apiKey);

        request.Content = JsonContent.Create(requestBody);
        
        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(json);
        return doc.RootElement
            .GetProperty("choices")[0]
            .GetProperty("message")
            .GetProperty("content")
            .GetString();
    }

    private static List<object> BuildMessages(
        string systemPrompt,
        List<(string role, string content)> messages)
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