using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lingafon.API.Controllers;

[ApiController]
[Route("api/messages")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _service;

    public MessageController(IMessageService service)
    {
        _service = service;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromQuery] Guid? dialogId)
    {
        if (dialogId.HasValue)
        {
            var byDialog = await _service.GetByDialogAsync(dialogId.Value);
            return Ok(byDialog);
        }

        var messages = await _service.GetAllAsync();
        return Ok(messages);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var message = await _service.GetByIdAsync(id);
        return Ok(message);
    }

    [HttpPost("")]
    public async Task<IActionResult> CreateMessage([FromBody] MessageCreateDto message)
    {
        var created = await _service.CreateAsync(message);
        return Ok(created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMessage([FromBody] MessageCreateDto messageUpdate)
    {
        await _service.UpdateAsync(messageUpdate);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(Guid id)
    {
        var isDeleted = await _service.DeleteAsync(id);
        return Ok(isDeleted);
    }
}

