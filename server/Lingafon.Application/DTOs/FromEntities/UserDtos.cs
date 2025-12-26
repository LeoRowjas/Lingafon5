using Lingafon.Core.Enums;

namespace Lingafon.Application.DTOs.FromEntities;

public record UserReadDto(Guid Id, string FirstName, string LastName, string Email, UserRole Role, string AvatarUrl, DateTime CreatedAt);

public record UserCreateDto
{
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public UserRole Role { get; init; }
}

public record UserUpdateDto
{
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? AvatarUrl { get; init; }
}

public record UserStatusDto
{
    public bool IsOnline { get; set; }
    public DateTime? LastSeenAt { get; set; }
}

