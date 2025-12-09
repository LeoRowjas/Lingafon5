using System.ComponentModel.DataAnnotations;
using Lingafon.Core.Enums;

namespace Lingafon.Core.Entities;

public class User : BaseEntity
{
    [Required] 
    [MaxLength(100)] 
    public string FirstName { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string MiddleName { get; set; } = string.Empty;
    [Required]
    [MaxLength(100)]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    [Required] 
    public UserRole Role { get; set; }
    
    public string AvatarUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}