using Lingafon.Core.Entities;
using Lingafon.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lingafon.Infrastructure.Persistence.Repositories;

public class AssignmentRepository : IAssignmentRepository
{
    private readonly LingafonDbContext _context;

    public AssignmentRepository(LingafonDbContext context)
    {
        _context = context;
    }

    public async Task<Assignment?> GetByIdAsync(Guid id)
    {
        return await _context.Assignments.FindAsync(id);
    }

    public async Task<IEnumerable<Assignment>> GetAllAsync()
    {
        return await _context.Assignments.ToListAsync();
    }

    public async Task<Assignment> AddAsync(Assignment entity)
    {
        await _context.Assignments.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(Assignment entity)
    {
        var exists = await _context.Assignments.AnyAsync(a => a.Id == entity.Id);
        if (!exists)
            return false;
        
        _context.Assignments.Update(entity);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var assignment = await _context.Assignments.FindAsync(id);
        if (assignment == null)
            return false;
        
        _context.Assignments.Remove(assignment);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Assignment>?> GetTeachersAssignmentsAsync(Guid teacherId)
    {
        return await _context.Assignments
            .Where(a => a.TeacherId == teacherId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Assignment>?> GetStudentsAssignmentsAsync(Guid studentId)
    {
        return await _context.Assignments
            .Where(a => a.StudentId == studentId)
            .ToListAsync();
    }
}