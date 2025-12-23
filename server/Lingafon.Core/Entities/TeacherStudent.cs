using System.ComponentModel.DataAnnotations;

namespace Lingafon.Core.Entities;

public class TeacherStudent : BaseEntity
{
    [Required]
    public Guid TeacherId { get; set; }
    [Required]
    public Guid StudentId { get; set; }
    public DateTime CreatedAt { get; set; }
}