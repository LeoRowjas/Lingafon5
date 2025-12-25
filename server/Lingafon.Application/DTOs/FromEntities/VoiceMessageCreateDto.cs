namespace Lingafon.Application.DTOs.FromEntities;

public class VoiceMessageCreateDto
{
    public Guid DialogId { get; set; }
    public Guid SenderId { get; set; }
    public Stream AudioStream { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string ContentType { get; set; } = null!;
}