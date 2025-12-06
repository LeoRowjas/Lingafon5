using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lingafon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AssignmentResultController : ControllerBase
{
    private readonly IAssignmentResultService _service;

    public AssignmentResultController(IAssignmentResultService service)
    {
        _service = service;
    }

    [Authorize]
    [HttpGet("")]
    public async Task<IActionResult> GetAll(
        [FromQuery] Guid? assignmentId,
        [FromQuery] Guid? studentId)
    {
        if (assignmentId.HasValue && studentId.HasValue)
        {
            return BadRequest("Укажите только один фильтр: assignmentId или studentId");
        }

        if (assignmentId.HasValue)
        {
            var byAssignment = await _service.GetByAssignmentAsync(assignmentId.Value);
            return Ok(byAssignment);
        }

        if (studentId.HasValue)
        {
            var byStudent = await _service.GetByStudentAsync(studentId.Value);
            return Ok(byStudent);
        }

        var results = await _service.GetAllAsync();
        return Ok(results);
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _service.GetByIdAsync(id);
        return Ok(result);
    }

    [Authorize]
    [HttpPost("")]
    public async Task<IActionResult> CreateAssignmentResult([FromBody] AssignmentResultCreateDto result)
    {
        var created = await _service.CreateAsync(result);
        return Ok(created);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAssignmentResult([FromBody] AssignmentResultCreateDto resultUpdate)
    {
        await _service.UpdateAsync(resultUpdate);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAssignmentResult(Guid id)
    {
        var isDeleted = await _service.DeleteAsync(id);
        return Ok(isDeleted);
    }
}

