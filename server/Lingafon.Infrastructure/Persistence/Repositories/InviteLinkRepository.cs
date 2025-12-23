using Lingafon.Core.Entities;
using Lingafon.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lingafon.Infrastructure.Persistence.Repositories;

public class InviteLinkRepository : IInviteLinkRepository
{
    private readonly LingafonDbContext dbContext;

    public InviteLinkRepository(LingafonDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<InviteLink?> GetByIdAsync(Guid id)
    {
        return await dbContext.InviteLinks.FindAsync(id);
    }

    public async Task<IEnumerable<InviteLink>> GetAllAsync()
    {
        return await dbContext.InviteLinks.ToListAsync();
    }

    public async Task<InviteLink> AddAsync(InviteLink entity)
    {
        await dbContext.InviteLinks.AddAsync(entity);
        await dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(InviteLink entity)
    {
        var exists = await dbContext.InviteLinks.FindAsync(entity.Id);
        if (exists == null)
            return false;
        dbContext.InviteLinks.Update(entity);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var exists = await dbContext.InviteLinks.FindAsync(id);
        if (exists == null)
            return false;
        dbContext.InviteLinks.Remove(exists);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<InviteLink?> GetByTokenAsync(string token)
    {
        var invite = await dbContext.InviteLinks
            .FirstAsync(x => x.Token == token);
        
        return invite;
    }

    public async Task<IEnumerable<InviteLink>?> GetExpiredInviteLinksAsync()
    {
        var invites = await dbContext.InviteLinks
            .Where(x => x.ExpiresAt < DateTime.UtcNow)
            .ToListAsync();
        
        return invites;
    }

    public async Task<IEnumerable<InviteLink>?> GetUsedInviteLinksAsync()
    {
        var invites = await dbContext.InviteLinks
            .Where(x => x.IsUsed == true)
            .ToListAsync();
        
        return invites;
    }

    public async Task<IEnumerable<InviteLink>?> GetByTeacherIdAsync(Guid teacherId)
    {
        var invites = await dbContext.InviteLinks
            .Where(x => x.TeacherId == teacherId)
            .ToListAsync();
        
        return invites;
    }
}