using Lingafon.Core.Entities;
using Lingafon.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lingafon.Infrastructure.Persistence.Repositories;

public class DialogRepository : IDialogRepository
{
    private LingafonDbContext _context;
    public DialogRepository(LingafonDbContext context)
    {
        _context = context;
    }
    
    public async Task<Dialog?> GetByIdAsync(Guid id)
    {
        return await _context.Dialogs.FindAsync(id);
    }

    public async Task<IEnumerable<Dialog>> GetAllAsync()
    {
        return await _context.Dialogs.ToListAsync();
    }

    public async Task<Dialog> AddAsync(Dialog entity)
    { 
        await _context.Dialogs.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Dialog entity)
    {
        var exists = await _context.Dialogs.AnyAsync(a => a.Id == entity.Id);
        if (!exists)
            return false;
        
        _context.Dialogs.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var dialog = await _context.Dialogs.FindAsync(id);
        if (dialog == null)
            return false;
        
        _context.Dialogs.Remove(dialog);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Dialog>?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Dialogs
            .Where(x => x.FirstUserId == userId || x.SecondUserId == userId)
            .ToListAsync();
    }

    public async Task<Dialog?> GetByParticipantsAsync(Guid firstUserId, Guid secondUserId)
    {
        return await _context.Dialogs
            .Where(x => (x.FirstUserId == firstUserId && x.SecondUserId == secondUserId)
            || (x.FirstUserId == secondUserId && x.SecondUserId == firstUserId))
            .FirstOrDefaultAsync();
    }
}