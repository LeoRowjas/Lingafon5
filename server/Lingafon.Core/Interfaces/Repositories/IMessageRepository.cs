using Lingafon.Core.Entities;

namespace Lingafon.Core.Interfaces.Repositories;

public interface IMessageRepository : IRepository<Message, Guid>
{
    Task<IEnumerable<Message>?> GetByDialogIdAsync(Guid dialogId);
    Task<IEnumerable<Message>?> GetBySenderIdAsync(Guid senderId);
}

