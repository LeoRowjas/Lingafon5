using Lingafon.Core.Entities;

namespace Lingafon.Core.Interfaces.Repositories;

public interface IAssignmentRepository : IRepository<Assignment, Guid>
{
    Task<IEnumerable<Assignment>?> GetTeachersAssignmentsAsync(Guid teacherId);
    Task<IEnumerable<Assignment>?> GetStudentsAssignmentsAsync(Guid studentId);
}