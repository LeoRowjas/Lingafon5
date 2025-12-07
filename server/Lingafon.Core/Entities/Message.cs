using System.ComponentModel.DataAnnotations;

namespace Lingafon.Core.Entities;

public class Message : BaseEntity
{
    [Required]
    [MaxLength(500)]
    public string Content { get; set; }
    public DateTime SentAt { get; set; }
    public bool IsFromAi { get; set; }
    [Required]
    public Guid DialogId { get; set; }
    [Required]
    public Guid SenderId { get; set; } //null if ai sender
}