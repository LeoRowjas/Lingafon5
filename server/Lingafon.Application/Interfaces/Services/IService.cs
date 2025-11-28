namespace Lingafon.Application.Interfaces.Services;

public interface IService<TReadDto, TCreateDto, TUpdateDto>
{
    Task<TReadDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<TReadDto>> GetAllAsync();
    Task<TReadDto> CreateAsync(TCreateDto dto);
    Task UpdateAsync(Guid id, TUpdateDto dto);
    Task DeleteAsync(Guid id);
}

