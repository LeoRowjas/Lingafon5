using Lingafon.Core.Entities;
using Lingafon.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lingafon.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly LingafonDbContext _context;

    public UserRepository(LingafonDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> AddAsync(User entity)
    {
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(User entity)
    {
        var exists = await _context.Users.AnyAsync(u => u.Id == entity.Id);
        if (!exists)
            return false;
        
        _context.Users.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;
        
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<string> UpdateAvatarUrlAsync(Guid id, string avatarUrl)
    {
        var user = await _context.Users.FindAsync(id);
        user!.AvatarUrl = avatarUrl;
        await _context.SaveChangesAsync();
        return avatarUrl;
    }

    public async Task<bool> DeleteAvatarAsync(Guid id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            return false;
        user!.AvatarUrl = string.Empty;
        await _context.SaveChangesAsync();
        return true;
    }
}