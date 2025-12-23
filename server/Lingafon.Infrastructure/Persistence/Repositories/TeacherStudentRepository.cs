using Lingafon.Core.Entities;
using Lingafon.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Lingafon.Infrastructure.Persistence.Repositories;

public class TeacherStudentRepository : ITeacherStudentRepository
{
    private readonly LingafonDbContext _dbContext;

    public TeacherStudentRepository(LingafonDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TeacherStudent?> GetByIdAsync(Guid id)
    {
        return await _dbContext.TeacherStudents.FindAsync(id);
    }

    public async Task<IEnumerable<TeacherStudent>> GetAllAsync()
    {
        return await _dbContext.TeacherStudents.ToListAsync();
    }

    public async Task<TeacherStudent> AddAsync(TeacherStudent entity)
    {
        await _dbContext.TeacherStudents.AddAsync(entity);
        await _dbContext.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> UpdateAsync(TeacherStudent entity)
    {
        var exists = await GetByIdAsync(entity.Id);
        if (exists is null)
            return false;
        _dbContext.TeacherStudents.Update(entity);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var exists = await GetByIdAsync(id);
        if (exists is null)
            return false;
        _dbContext.TeacherStudents.Remove(exists);
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<TeacherStudent>?> GetAllFotTeacherAsync(Guid teacherId)
    {
        var teacher = await _dbContext.InviteLinks.FindAsync(teacherId);
        if (teacher is null)
            return null;

        var all = await _dbContext.TeacherStudents
            .Where(x => x.TeacherId == teacherId)
            .ToListAsync();
        
        return all;
    }
}