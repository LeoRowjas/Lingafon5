using Lingafon.Core.Entities;

namespace Lingafon.Core.Interfaces.Repositories;

public interface IDialogRepository : IRepository<Dialog, Guid>
{
    Task<IEnumerable<Dialog>?> GetByUserIdAsync(Guid userId);
    Task<Dialog?> GetByParticipantsAsync(Guid firstUserId, Guid secondUserId);
}

