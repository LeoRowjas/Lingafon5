using Lingafon.Core.Enums;

namespace Lingafon.Application.DTOs.FromEntities;

public record AssignmentResultReadDto(Guid Id, int Grade, Guid AssignmentId, Guid StudentId, DateTime SubmittedAt, string? Feedback, AssignmentStatus Status);

public record AssignmentResultCreateDto
{
    public int Grade { get; init; }
    public Guid AssignmentId { get; init; }
    public Guid StudentId { get; init; }
    public string? Feedback { get; init; }
}

