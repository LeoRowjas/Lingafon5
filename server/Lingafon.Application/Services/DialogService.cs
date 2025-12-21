using AutoMapper;
using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.Interfaces.Services;
using Lingafon.Core.Entities;
using Lingafon.Core.Enums;
using Lingafon.Core.Interfaces.Repositories;

namespace Lingafon.Application.Services;

public class DialogService : IDialogService
{
    private readonly IDialogRepository _repository;
    private readonly IMapper _mapper;

    public DialogService(IDialogRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<DialogReadDto?> GetByIdAsync(Guid id)
    {
        var dialog = await _repository.GetByIdAsync(id);
        if (dialog is null)
            return null;
        return _mapper.Map<DialogReadDto>(dialog);
    }

    public async Task<IEnumerable<DialogReadDto>> GetAllAsync()
    {
        var dialogs = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<DialogReadDto>>(dialogs);
    }

    public async Task<DialogReadDto> CreateAsync(DialogCreateDto dto)
    {
        var dialog = _mapper.Map<Dialog>(dto);
        await _repository.AddAsync(dialog);
        return _mapper.Map<DialogReadDto>(dialog);
    }

    public async Task UpdateAsync(DialogCreateDto dto)
    {
        var dialog = _mapper.Map<Dialog>(dto);
        await _repository.UpdateAsync(dialog);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<DialogReadDto>> GetForUserAsync(Guid userId)
    {
        var dialogs = await _repository.GetByUserIdAsync(userId);
        return _mapper.Map<IEnumerable<DialogReadDto>>(dialogs);
    }

    public async Task<DialogReadDto> CreateWithAiAsync(DialogCreateWithAiDto dto, Guid userId)
    {
        var dialog = new Dialog
        {
            Title = dto.Title,
            Type = DialogType.Ai,
            FirstUserId = userId,
            SecondUserId = Guid.Empty,
            CreatedAt = DateTime.UtcNow
        };
        await _repository.AddAsync(dialog);
        return _mapper.Map<DialogReadDto>(dialog);
    }
}

