using AutoMapper;
using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.Interfaces.Services;
using Lingafon.Core.Entities;
using Lingafon.Core.Interfaces.Repositories;

namespace Lingafon.Application.Services;

public class AssignmentResultService : IAssignmentResultService
{
    private readonly IAssignmentResultRepository _repository;
    private readonly IMapper _mapper;

    public AssignmentResultService(IAssignmentResultRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AssignmentResultReadDto?> GetByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        var result = await _repository.GetByIdAsync(id);
        if (result is null)
            return null;
        return _mapper.Map<AssignmentResultReadDto>(result);
    }

    public async Task<IEnumerable<AssignmentResultReadDto>> GetAllAsync()
    {
        var results = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<AssignmentResultReadDto>>(results);
    }

    public async Task<AssignmentResultReadDto> CreateAsync(AssignmentResultCreateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var result = _mapper.Map<AssignmentResult>(dto);
        await _repository.AddAsync(result);
        return _mapper.Map<AssignmentResultReadDto>(result);
    }

    public async Task UpdateAsync(AssignmentResultCreateDto dto)
    {
        if (dto == null)
            throw new ArgumentNullException(nameof(dto));

        var result = _mapper.Map<AssignmentResult>(dto);
        await _repository.UpdateAsync(result);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));

        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<AssignmentResultReadDto>> GetByAssignmentAsync(Guid assignmentId)
    {
        if (assignmentId == Guid.Empty)
            throw new ArgumentException("AssignmentId cannot be empty", nameof(assignmentId));

        var results = await _repository.GetByAssignmentIdAsync(assignmentId);
        return _mapper.Map<IEnumerable<AssignmentResultReadDto>>(results);
    }

    public async Task<IEnumerable<AssignmentResultReadDto>> GetByStudentAsync(Guid studentId)
    {
        if (studentId == Guid.Empty)
            throw new ArgumentException("StudentId cannot be empty", nameof(studentId));

        var results = await _repository.GetByStudentIdAsync(studentId);
        return _mapper.Map<IEnumerable<AssignmentResultReadDto>>(results);
    }
}

