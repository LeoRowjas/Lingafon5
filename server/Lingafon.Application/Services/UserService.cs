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
    private readonly IOnlineStatusService _statusService;
    private readonly StorageSettings _storageSettings;

    public UserService(
        IUserRepository repository,
        IMapper mapper, 
        IFileStorageService storageService,
        IOptions<StorageSettings> storageSettings, 
        IOnlineStatusService statusService)
    {
        _repository = repository;
        _mapper = mapper;
        _storageService = storageService;
        _statusService = statusService;
        _storageSettings = storageSettings.Value;
    }

    public async Task<UserReadDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        var user = await _repository.GetByIdAsync(id);
        return user is null ? null : _mapper.Map<UserReadDto>(user);
    }

    public async Task<IEnumerable<UserReadDto>> GetAllAsync()
    {
        var users = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserReadDto>>(users);
    }
    
    public async Task<UserStatusDto?> GetStatusAsync(Guid userId)
    {
        var user = await _repository.GetByIdAsync(userId);
        var isOnline = _statusService.IsOnline(userId);
        
        return new UserStatusDto
        {
            IsOnline = isOnline,
            LastSeenAt = isOnline ? null : user.LastSeenAt,
        };
    }

    public async Task<UserReadDto> CreateAsync(UserCreateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var user = _mapper.Map<User>(dto);
        await _repository.AddAsync(user);
        return _mapper.Map<UserReadDto>(user);
    }

    public async Task UpdateAsync(UserUpdateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var user = _mapper.Map<User>(dto);
        await _repository.UpdateAsync(user);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        return await _repository.DeleteAsync(id);
    }

    public async Task<UserReadDto?> GetByEmailAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty", nameof(email));

        var user = await _repository.GetByEmailAsync(email);
        if (user is null)
            return null;
        return _mapper.Map<UserReadDto>(user);
    }

    public async Task<string?> UpdateAvatarUrlAsync(Guid id, Stream fileStream, string fileName, string contentType)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));
        if (fileStream == null)
            throw new ArgumentNullException(nameof(fileStream));
        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("FileName cannot be empty", nameof(fileName));
        if (string.IsNullOrWhiteSpace(contentType))
            throw new ArgumentException("ContentType cannot be empty", nameof(contentType));

        var user = await _repository.GetByIdAsync(id);
        if (user is null)
            return null;

        if (!string.IsNullOrEmpty(user.AvatarUrl))
            await _storageService.DeleteFileAsync(user.AvatarUrl, _storageSettings.BucketNameAvatars);

        var path = await _storageService.UploadFileAsync(fileStream, fileName, contentType, _storageSettings.BucketNameAvatars);
        await _repository.UpdateAvatarUrlAsync(id, path);

        return path;
    }

    public async Task<bool> DeleteAvatarAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        var user = await _repository.GetByIdAsync(id);
        if (user is null)
            return false; 
        var fileName = user.AvatarUrl.Split('/').Last();
        await _storageService.DeleteFileAsync(fileName, _storageSettings.BucketNameAvatars);
        return await _repository.DeleteAvatarAsync(id);
    }
}