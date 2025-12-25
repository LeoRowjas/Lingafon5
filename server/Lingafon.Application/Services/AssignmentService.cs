using AutoMapper;
using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.Interfaces.Services;
using Lingafon.Core.Entities;
using Lingafon.Core.Enums;
using Lingafon.Core.Interfaces.Repositories;

namespace Lingafon.Application.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _repository;
    private IMapper _mappingProfile;

    public AssignmentService(IAssignmentRepository repository, IMapper mappingProfile)
    {
        _repository = repository;
        _mappingProfile = mappingProfile;
    }

    public async Task<AssignmentReadDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        var responseFromDb = await _repository.GetByIdAsync(id);
        if (responseFromDb is null)
            return null;
        var readDto = _mappingProfile.Map<AssignmentReadDto>(responseFromDb);
        return readDto;
    }

    public async Task<IEnumerable<AssignmentReadDto>> GetAllAsync()
    {
        var responseFromDb = await _repository.GetAllAsync();
        return _mappingProfile.Map<IEnumerable<AssignmentReadDto>>(responseFromDb);
    }

    public async Task<AssignmentReadDto> CreateAsync(AssignmentCreateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var assignment = _mappingProfile.Map<Assignment>(dto);
        await _repository.AddAsync(assignment);
        return _mappingProfile.Map<AssignmentReadDto>(assignment);
    }

    public async Task UpdateAsync(AssignmentUpdateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));
        if (dto.Id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(dto.Id));

        var assignment = _mappingProfile.Map<Assignment>(dto);
        await _repository.UpdateAsync(assignment);
    }
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        var isDeleted = await _repository.DeleteAsync(id);
        return isDeleted;
    }

    public async Task<IEnumerable<AssignmentReadDto>> GetForTeacherAsync(Guid teacherId)
    {
        if (teacherId == Guid.Empty)
            throw new ArgumentException("TeacherId cannot be empty", nameof(teacherId));

        var assignments = await _repository.GetTeachersAssignmentsAsync(teacherId);
        var mapped = _mappingProfile.Map<IEnumerable<AssignmentReadDto>>(assignments);
        return mapped;
    }

    public async Task<IEnumerable<AssignmentReadDto>> GetForStudentAsync(Guid studentId)
    {
        if (studentId == Guid.Empty)
            throw new ArgumentException("StudentId cannot be empty", nameof(studentId));

        var assignments = await _repository.GetStudentsAssignmentsAsync(studentId);
        var mapped = _mappingProfile.Map<IEnumerable<AssignmentReadDto>>(assignments);
        return mapped;
    }

    public async Task<bool> UpdateStatusAsync(Guid id, AssignmentStatus status)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        return await _repository.UpdateStatusAsync(id, status);
    }
}