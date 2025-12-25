namespace Lingafon.Application.DTOs;

public record AiReplyResponse
{
    public string Reply { get; set; } = string.Empty;
    public string? AudioUrl { get; set; }
    public Guid DialogId { get; set; }
    public DateTime ReplyAt { get; set; }
    public bool Success { get; set; }
    public string? ErrorMessage { get; set; }
}

