using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lingafon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DialogController : ControllerBase
{
    private readonly IDialogService _service;

    public DialogController(IDialogService service)
    {
        _service = service;
    }

    private Guid GetUserIdFromClaims()
    {
        var userIdClaim = User.FindFirst("sub")?.Value ?? User.FindFirst("nameid")?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User ID not found in claims");
        }
        return userId;
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

    [Authorize]
    [HttpPost("")]
    public async Task<IActionResult> CreateDialog([FromBody] DialogCreateDto dialog)
    {
        var created = await _service.CreateAsync(dialog);
        return Ok(created);
    }

    [Authorize]
    [HttpPost("with-user")]
    public async Task<IActionResult> CreateDialogWithUser([FromBody] DialogCreateWithUserDto dto)
    {
        var currentUserId = GetUserIdFromClaims();
        var dialogDto = new DialogCreateDto
        {
            Title = dto.Title,
            Type = dto.Type,
            FirstUserId = currentUserId,
            SecondUserId = dto.SecondUserId
        };
        var created = await _service.CreateAsync(dialogDto);
        return Ok(created);
    }

    [Authorize]
    [HttpPost("ai")]
    public async Task<IActionResult> CreateDialogWithAi([FromBody] DialogCreateWithAiDto dto)
    {
        var currentUserId = GetUserIdFromClaims();
        var created = await _service.CreateWithAiAsync(dto, currentUserId);
        return Ok(created);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDialog([FromBody] DialogCreateDto dialogUpdate)
    {
        await _service.UpdateAsync(dialogUpdate);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDialog(Guid id)
    {
        var isDeleted = await _service.DeleteAsync(id);
        return Ok(isDeleted);
    }
}

