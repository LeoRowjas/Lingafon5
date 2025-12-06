using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.Interfaces.Services;
using Lingafon.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lingafon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssignmentController : ControllerBase
{
    private IAssignmentService _service;

    public AssignmentController(IAssignmentService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet("")]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? teacherId,
        [FromQuery] Guid? studentId)
    {
        if (teacherId.HasValue && studentId.HasValue)
        {
            return BadRequest("Укажите только один фильтр: teacherId или studentId");
        }

        if (teacherId.HasValue)
        {
            var byTeacher = await _service.GetForTeacherAsync(teacherId.Value);
            return Ok(byTeacher);
        }

        if (studentId.HasValue)
        {
            var byStudent = await _service.GetForStudentAsync(studentId.Value);
            return Ok(byStudent);
        }

        var all = await _service.GetAllAsync();
        return Ok(all);
    }
    
    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var assignment = await _service.GetByIdAsync(id);
        return Ok(assignment);
    }

    [Authorize]
    [HttpPost("")]
    public async Task<IActionResult> CreateAssignment([FromBody] AssignmentCreateDto assignment)
    {
        var creation = await _service.CreateAsync(assignment);
        return Ok(creation);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssignment([FromBody] AssignmentUpdateDto assignmentUpdate)
    {
        var updated = _service.UpdateAsync(assignmentUpdate);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssignment(Guid id)
    {
        var isDeleted = await _service.DeleteAsync(id);
        return Ok(isDeleted);
    }
    
    [HttpPut("{id}/status/update")]
    public async Task<IActionResult> UpdateAssignmentStatus(Guid id, [FromQuery]AssignmentStatus status)
    {
        var updated = await _service.UpdateStatusAsync(id, status);
        return Ok(updated);
    }
}