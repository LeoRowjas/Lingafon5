using AutoMapper;
using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.Interfaces.Services;
using Lingafon.Core.Entities;
using Lingafon.Core.Interfaces.Repositories;
using Lingafon.Core.Interfaces.Services;
using Microsoft.Extensions.Options;

namespace Lingafon.Application.Services;

public class UserService : IUserService
{
    private readonly IFileStorageService _storageService;
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repository, IMapper mapper, IFileStorageService storageService)
    {
        _repository = repository;
        _mapper = mapper;
        _storageService = storageService;
    }

    public async Task<UserReadDto?> GetByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        return user is null ? null : _mapper.Map<UserReadDto>(user);
    }

    public async Task<IEnumerable<UserReadDto>> GetAllAsync()
    {
        var users = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserReadDto>>(users);
    }

    public async Task<UserReadDto> CreateAsync(UserCreateDto dto)
    {
        var user = _mapper.Map<User>(dto);
        await _repository.AddAsync(user);
        return _mapper.Map<UserReadDto>(user);
    }

    public async Task UpdateAsync(UserUpdateDto dto)
    {
        var user = _mapper.Map<User>(dto);
        await _repository.UpdateAsync(user);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<UserReadDto?> GetByEmailAsync(string email)
    {
        var user = await _repository.GetByEmailAsync(email);
        if (user is null)
            return null;
        return _mapper.Map<UserReadDto>(user);
    }

    public async Task<string?> UpdateAvatarUrlAsync(Guid id, Stream fileStream, string fileName, string contentType)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user is null)
            return null;
        
        if(!string.IsNullOrEmpty(user.AvatarUrl))
            await _storageService.DeleteFileAsync(user.AvatarUrl);

        var path = await _storageService.UploadFileAsync(fileStream, fileName, contentType);
        await _repository.UpdateAvatarUrlAsync(id, path);
        
        return path;
    }

    public async Task<bool> DeleteAvatarAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user is null)
            return false;
        await _storageService.DeleteFileAsync(user.AvatarUrl);
        return await _repository.DeleteAvatarAsync(id);
    }
}

