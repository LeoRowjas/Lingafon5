namespace Lingafon.Application.DTOs;

public record AiReplyRequest
{
    public Guid DialogId { get; init; }
    public bool IncludeHistory { get; init; } = true;
    public int HistoryLimit { get; set; } = 10;
}

