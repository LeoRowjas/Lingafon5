using Lingafon.Core.Entities;
using Lingafon.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lingafon.Infrastructure.Persistence.Repositories;

public class AssignmentResultRepository : IAssignmentResultRepository
{
    private LingafonDbContext _context;
    public AssignmentResultRepository(LingafonDbContext context)
    {
        _context = context;
    }
    
    public async Task<AssignmentResult?> GetByIdAsync(Guid id)
    {
        return await _context.AssignmentResults.FindAsync(id);
    }

    public async Task<IEnumerable<AssignmentResult>> GetAllAsync()
    {
        return await _context.AssignmentResults.ToListAsync();
    }

    public async Task<AssignmentResult> AddAsync(AssignmentResult entity)
    {
        await _context.AssignmentResults.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(AssignmentResult entity)
    {
        var exists = await _context.AssignmentResults.AnyAsync(a => a.Id == entity.Id);
        if (!exists)
            return false;
        
        _context.AssignmentResults.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var assignmentResult = await _context.AssignmentResults.FindAsync(id);
        if (assignmentResult == null)
            return false;
        
        _context.AssignmentResults.Remove(assignmentResult);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<AssignmentResult>?> GetByAssignmentIdAsync(Guid assignmentId)
    {
        return await _context.AssignmentResults
            .Where(ar => ar.AssignmentId == assignmentId)
            .ToListAsync();
    }

    public async Task<IEnumerable<AssignmentResult>?> GetByStudentIdAsync(Guid studentId)
    {
        return await _context.AssignmentResults
            .Where(ar => ar.StudentId == studentId)
            .ToListAsync();
    }

    public async Task<AssignmentResult?> GetLatestByAssignmentAndStudentAsync(Guid assignmentId, Guid studentId)
    {
        return await _context.AssignmentResults
            .Where(ar => ar.AssignmentId == assignmentId && ar.StudentId == studentId)
            .OrderByDescending(ar => ar.SubmittedAt)
            .FirstOrDefaultAsync();
    }
}