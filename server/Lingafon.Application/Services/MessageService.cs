using AutoMapper;
using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.Interfaces.Services;
using Lingafon.Core.Entities;
using Lingafon.Core.Interfaces.Repositories;

namespace Lingafon.Application.Services;

public class MessageService : IMessageService
{
    private readonly IMessageRepository _repository;
    private readonly IMapper _mapper;

    public MessageService(IMessageRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<MessageReadDto?> GetByIdAsync(Guid id)
    {
        var message = await _repository.GetByIdAsync(id);
        if (message is null)
            return null;
        return _mapper.Map<MessageReadDto>(message);
    }

    public async Task<IEnumerable<MessageReadDto>> GetAllAsync()
    {
        var messages = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<MessageReadDto>>(messages);
    }

    public async Task<MessageReadDto> CreateAsync(MessageCreateDto dto)
    {
        var message = _mapper.Map<Message>(dto);
        await _repository.AddAsync(message);
        return _mapper.Map<MessageReadDto>(message);
    }

    public async Task UpdateAsync(MessageCreateDto dto)
    {
        var message = _mapper.Map<Message>(dto);
        await _repository.UpdateAsync(message);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<MessageReadDto>> GetByDialogAsync(Guid dialogId)
    {
        var messages = await _repository.GetByDialogIdAsync(dialogId);
        return _mapper.Map<IEnumerable<MessageReadDto>>(messages);
    }
}

