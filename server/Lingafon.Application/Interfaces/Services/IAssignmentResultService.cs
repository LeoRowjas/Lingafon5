using Lingafon.Application.DTOs.FromEntities;

namespace Lingafon.Application.Interfaces.Services;

public interface IAssignmentResultService : IService<AssignmentResultReadDto, AssignmentResultCreateDto, AssignmentResultCreateDto>
{
    Task<IEnumerable<AssignmentResultReadDto>> GetByAssignmentAsync(Guid assignmentId);
    Task<IEnumerable<AssignmentResultReadDto>> GetByStudentAsync(Guid studentId);
}
