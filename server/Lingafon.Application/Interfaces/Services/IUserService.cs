using Lingafon.Application.DTOs.FromEntities;

namespace Lingafon.Application.Interfaces.Services;

public interface IUserService : IService<UserReadDto, UserCreateDto, UserUpdateDto>
{
    Task<UserReadDto?> GetByEmailAsync(string email);
    Task<string?> UpdateAvatarUrlAsync(Guid id, Stream fileStream, string fileName, string contentType);
    Task<bool> DeleteAvatarAsync(Guid id);
    Task<UserStatusDto?> GetStatusAsync(Guid userId);
}
