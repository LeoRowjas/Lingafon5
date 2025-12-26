using Lingafon.Core.Entities;

namespace Lingafon.Core.Interfaces.Repositories;

public interface ITeacherStudentRepository : IRepository<TeacherStudent, Guid>
{
    Task<IEnumerable<TeacherStudent>?> GetAllFotTeacherAsync(Guid teacherId);
    Task<IEnumerable<TeacherStudent>?> GetAllFotStudentAsync(Guid studentId);
}