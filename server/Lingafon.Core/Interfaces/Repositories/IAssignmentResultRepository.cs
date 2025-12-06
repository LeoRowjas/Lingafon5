using Lingafon.Core.Entities;

namespace Lingafon.Core.Interfaces.Repositories;

public interface IAssignmentResultRepository : IRepository<AssignmentResult, Guid>
{
    Task<IEnumerable<AssignmentResult>?> GetByAssignmentIdAsync(Guid assignmentId);
    Task<IEnumerable<AssignmentResult>?> GetByStudentIdAsync(Guid studentId);
    Task<AssignmentResult?> GetLatestByAssignmentAndStudentAsync(Guid assignmentId, Guid studentId);
}