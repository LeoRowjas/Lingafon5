using System.ComponentModel.DataAnnotations;
using Lingafon.Core.Enums;

namespace Lingafon.Core.Entities;

public class AssignmentResult
{
    [Required]
    public int Grade { get; set; }
    [Required]
    public Guid AssignmentId { get; set; }
    [Required]
    public Guid StudentId { get; set; }
    
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;

    [MaxLength(500)] 
    public string? Feedback { get; set; } = string.Empty;
    public AssignmentStatus Status { get; set; }
    
}