using System.ComponentModel.DataAnnotations;

namespace Lingafon.Core.Entities;

public class Assignments : BaseEntity
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;
    [Required]
    public Guid TeacherId { get; set; }
    [Required]
    public Guid StudentId { get; set; }
    [MaxLength(500)]
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; set; }
}