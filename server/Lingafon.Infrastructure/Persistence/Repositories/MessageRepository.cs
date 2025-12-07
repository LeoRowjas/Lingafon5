using Lingafon.Core.Entities;
using Lingafon.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lingafon.Infrastructure.Persistence.Repositories;

public class MessageRepository : IMessageRepository
{
    private LingafonDbContext _context;
    public MessageRepository(LingafonDbContext context)
    {
        _context = context;
    }
    
    public async Task<Message?> GetByIdAsync(Guid id)
    {
        return await _context.Messages.FindAsync(id);
    }

    public async Task<IEnumerable<Message>> GetAllAsync()
    {
        return await _context.Messages.ToListAsync();
    }

    public async Task<Message> AddAsync(Message entity)
    {
        await  _context.Messages.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Message entity)
    {
        var exists =  await _context.Messages.FindAsync(entity.Id);
        if (exists == null)
            return false;
        
        _context.Messages.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var entity = await _context.Messages.FindAsync(id);
        if (entity == null)
            return false;
        
        _context.Messages.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Message>?> GetByDialogIdAsync(Guid dialogId)
    {
        return await _context.Messages.Where(m => m.DialogId == dialogId).ToListAsync();
    }

    public async Task<IEnumerable<Message>?> GetBySenderIdAsync(Guid senderId)
    {
        return await _context.Messages.Where(m => m.SenderId == senderId).ToListAsync();
    }
}