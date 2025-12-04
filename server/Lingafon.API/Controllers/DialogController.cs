using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lingafon.API.Controllers;

[ApiController]
[Route("api/dialogs")]
public class DialogController : ControllerBase
{
    private readonly IDialogService _service;

    public DialogController(IDialogService service)
    {
        _service = service;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] Guid? userId)
    {
        if (userId.HasValue)
        {
            var byUser = await _service.GetForUserAsync(userId.Value);
            return Ok(byUser);
        }

        var dialogs = await _service.GetAllAsync();
        return Ok(dialogs);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var dialog = await _service.GetByIdAsync(id);
        return Ok(dialog);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateDialog([FromBody] DialogCreateDto dialog)
    {
        var created = await _service.CreateAsync(dialog);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDialog([FromBody] DialogCreateDto dialogUpdate)
    {
        await _service.UpdateAsync(dialogUpdate);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDialog(Guid id)
    {
        var isDeleted = await _service.DeleteAsync(id);
        return Ok(isDeleted);
    }
}

