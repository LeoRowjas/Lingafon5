namespace Lingafon.Application.DTOs.FromEntities;

public record AssignmentReadDto(Guid Id, string Title, Guid TeacherId, Guid StudentId, string? Description, DateTime CreatedAt, DateTime? DueDate);

public record AssignmentCreateDto
{
    public string Title { get; init; } = string.Empty;
    public Guid TeacherId { get; init; }
    public Guid StudentId { get; init; }
    public string? Description { get; init; }
    public DateTime? DueDate { get; init; }
}

public record AssignmentUpdateDto
{
    public Guid Id { get; init; }
    public string? Title { get; init; }
    public string? Description { get; init; }
    public DateTime? DueDate { get; init; }
}

