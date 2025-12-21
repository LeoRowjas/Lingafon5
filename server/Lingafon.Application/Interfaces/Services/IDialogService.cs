using Lingafon.Application.DTOs.FromEntities;

namespace Lingafon.Application.Interfaces.Services;

public interface IDialogService : IService<DialogReadDto, DialogCreateDto, DialogCreateDto>
{
    Task<IEnumerable<DialogReadDto>> GetForUserAsync(Guid userId);
    Task<DialogReadDto> CreateWithAiAsync(DialogCreateWithAiDto dto, Guid userId);
}
