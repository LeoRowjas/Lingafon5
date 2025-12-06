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
        var result = _mapper.Map<AssignmentResult>(dto);
        await _repository.AddAsync(result);
        return _mapper.Map<AssignmentResultReadDto>(result);
    }

    public async Task UpdateAsync(AssignmentResultCreateDto dto)
    {
        var result = _mapper.Map<AssignmentResult>(dto);
        await _repository.UpdateAsync(result);
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<AssignmentResultReadDto>> GetByAssignmentAsync(Guid assignmentId)
    {
        var results = await _repository.GetByAssignmentIdAsync(assignmentId);
        return _mapper.Map<IEnumerable<AssignmentResultReadDto>>(results);
    }

    public async Task<IEnumerable<AssignmentResultReadDto>> GetByStudentAsync(Guid studentId)
    {
        var results = await _repository.GetByStudentIdAsync(studentId);
        return _mapper.Map<IEnumerable<AssignmentResultReadDto>>(results);
    }
}

