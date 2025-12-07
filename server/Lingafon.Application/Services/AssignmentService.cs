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
        var assignment = _mappingProfile.Map<Assignment>(dto);
        await _repository.AddAsync(assignment);
        return _mappingProfile.Map<AssignmentReadDto>(assignment);
    }

    public async Task UpdateAsync(AssignmentUpdateDto dto)
    {
        var assignment = _mappingProfile.Map<Assignment>(dto);
        await _repository.UpdateAsync(assignment);
    }
    
    public async Task<bool> DeleteAsync(Guid id)
    {
        var isDeleted = await _repository.DeleteAsync(id);
        return isDeleted;
    }

    public async Task<IEnumerable<AssignmentReadDto>> GetForTeacherAsync(Guid teacherId)
    {
        var assignments = await _repository.GetTeachersAssignmentsAsync(teacherId);
        var mapped = _mappingProfile.Map<IEnumerable<AssignmentReadDto>>(assignments);
        return mapped;
    }

    public async Task<IEnumerable<AssignmentReadDto>> GetForStudentAsync(Guid studentId)
    {
        var assignments = await _repository.GetStudentsAssignmentsAsync(studentId);
        var mapped = _mappingProfile.Map<IEnumerable<AssignmentReadDto>>(assignments);
        return mapped;
    }

    public async Task<bool> UpdateStatusAsync(Guid id, AssignmentStatus status)
    {
        return await _repository.UpdateStatusAsync(id, status);
    }
}