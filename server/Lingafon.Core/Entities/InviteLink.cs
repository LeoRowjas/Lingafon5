using System.ComponentModel.DataAnnotations;

namespace Lingafon.Core.Entities;

public class InviteLink : BaseEntity
{
    [Required]
    public Guid TeacherId { get; set; }
    [Required]
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
    public DateTime CreatedAt { get; set; }
}