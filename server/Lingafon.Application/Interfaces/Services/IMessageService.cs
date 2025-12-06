using Lingafon.Application.DTOs.FromEntities;

namespace Lingafon.Application.Interfaces.Services;

public interface IMessageService : IService<MessageReadDto, MessageCreateDto, MessageCreateDto>
{
    Task<IEnumerable<MessageReadDto>> GetByDialogAsync(Guid dialogId);
}
