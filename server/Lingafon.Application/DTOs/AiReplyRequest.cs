namespace Lingafon.Application.DTOs;

public record AiReplyRequest
{
    public Guid DialogId { get; set; }
    public string UserMessage { get; set; } = string.Empty;
    public string? SystemPrompt { get; set; }
    public bool IncludeHistory { get; set; } = true;
    public int HistoryLimit { get; set; } = 10;
}

