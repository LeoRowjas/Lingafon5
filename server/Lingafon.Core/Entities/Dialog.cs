using System.ComponentModel.DataAnnotations;
using Lingafon.Core.Enums;

namespace Lingafon.Core.Entities;

public class Dialog : BaseEntity
{
    [Required]
    [MaxLength(150)]
    public string Title { get; set; } = string.Empty;
    [Required]
    public DialogType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    [Required]
    public Guid FirstUserId { get; set; } 
    //TODO:public User? FirstUser { get; set; } можно попробовать после получения айдишника сразу сюда пихать модель
    [Required]
    public Guid SecondUserId { get; set; }
    //TODO:public User? SecondUser { get; set; }
}