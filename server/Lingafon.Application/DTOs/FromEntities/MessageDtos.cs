namespace Lingafon.Application.DTOs.FromEntities;

public record MessageReadDto(Guid Id, string Content, DateTime SentAt, bool IsFromAi, Guid DialogId, Guid SenderId);

public record MessageCreateDto
{
    public string Content { get; init; } = string.Empty;
    public bool IsFromAi { get; init; }
    public Guid DialogId { get; init; }
    public Guid SenderId { get; init; }
}

