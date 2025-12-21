using Lingafon.Core.Enums;

namespace Lingafon.Application.DTOs.FromEntities;

public record DialogReadDto(Guid Id, string Title, DialogType Type, DateTime CreatedAt, Guid FirstUserId, Guid SecondUserId);

public record DialogCreateDto
{
    public string Title { get; init; } = string.Empty;
    public DialogType Type { get; init; }
    public Guid FirstUserId { get; init; }
    public Guid SecondUserId { get; init; }
}

public record DialogCreateWithAiDto
{
    public string Title { get; init; } = string.Empty;
}

public record DialogCreateWithUserDto
{
    public string Title { get; init; } = string.Empty;
    public DialogType Type { get; init; }
    public Guid SecondUserId { get; init; }
}

