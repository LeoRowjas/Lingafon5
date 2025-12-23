using Lingafon.Core.Entities;

namespace Lingafon.Core.Interfaces.Repositories;

public interface IInviteLinkRepository : IRepository<InviteLink, Guid>
{
    Task<InviteLink?> GetByTokenAsync(string token);
    Task<IEnumerable<InviteLink>?> GetExpiredInviteLinksAsync();
    Task<IEnumerable<InviteLink>?> GetUsedInviteLinksAsync();
    Task<IEnumerable<InviteLink>?> GetByTeacherIdAsync(Guid teacherId);
}