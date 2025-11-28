using Lingafon.Application.DTOs.FromEntities;

namespace Lingafon.Application.Interfaces.Services;

public interface IUserService : IService<UserReadDto, UserCreateDto, UserUpdateDto>
{
    Task<UserReadDto?> GetByEmailAsync(string email);
}
