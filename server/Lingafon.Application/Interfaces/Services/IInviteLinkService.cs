using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Core.Entities;

namespace Lingafon.Application.Interfaces.Services;

public interface IInviteLinkService : IService<InviteLinkReadDto, InviteLinkCreateDto, InviteLinkCreateDto>
{
    Task<bool> AcceptInviteLink(Guid userId, string inviteId);
    Task<IEnumerable<InviteLink>?> GetExpiredInviteLinksAsync();
    Task<IEnumerable<InviteLink>?> GetUsedInviteLinksAsync();
    Task<IEnumerable<InviteLink>?> GetByTeacherIdAsync(Guid teacherId);
}