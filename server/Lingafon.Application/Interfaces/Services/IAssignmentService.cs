using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Core.Enums;

namespace Lingafon.Application.Interfaces.Services;

public interface IAssignmentService : IService<AssignmentReadDto, AssignmentCreateDto, AssignmentUpdateDto>
{
    Task<IEnumerable<AssignmentReadDto>> GetForTeacherAsync(Guid teacherId);
    Task<IEnumerable<AssignmentReadDto>> GetForStudentAsync(Guid studentId);
    Task UpdateStatusAsync(Guid id, AssignmentStatus status);
}
