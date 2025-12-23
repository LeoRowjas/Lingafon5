namespace Lingafon.Application.DTOs.FromEntities;

public class InviteLinkReadDto
{
    public Guid TeacherId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
}

public class InviteLinkCreateDto
{
    public Guid TeacherId { get; set; }
    public DateTime ExpiresAt { get; set; }
}