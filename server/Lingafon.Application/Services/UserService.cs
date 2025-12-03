using AutoMapper;
using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.Interfaces.Services;
using Lingafon.Core.Entities;
using Lingafon.Core.Interfaces.Repositories;

namespace Lingafon.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;

    public UserService(IUserRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<UserReadDto?> GetByIdAsync(Guid id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user is null)
            return null;
        return _mapper.Map<UserReadDto>(user);
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
}

