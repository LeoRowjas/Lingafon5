using Lingafon.Core.Entities;
using Lingafon.Core.Enums;

namespace Lingafon.Core.Interfaces.Repositories;

public interface IAssignmentRepository : IRepository<Assignment, Guid>
{
    Task<IEnumerable<Assignment>?> GetTeachersAssignmentsAsync(Guid teacherId);
    Task<IEnumerable<Assignment>?> GetStudentsAssignmentsAsync(Guid studentId);
    Task<bool> UpdateStatusAsync(Guid assignmentId, AssignmentStatus status);
}