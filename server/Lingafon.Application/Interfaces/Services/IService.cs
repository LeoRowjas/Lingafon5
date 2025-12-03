namespace Lingafon.Application.Interfaces.Services;

public interface IService<TReadDto, TCreateDto, TUpdateDto>
{
    Task<TReadDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<TReadDto>> GetAllAsync();
    Task<TReadDto> CreateAsync(TCreateDto dto);
    Task UpdateAsync(TUpdateDto dto);
    Task<bool> DeleteAsync(Guid id);
}

