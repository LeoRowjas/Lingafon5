using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.DTOs;
using Lingafon.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Lingafon.API.Models;

namespace Lingafon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessageController : ControllerBase
{
    private readonly IMessageService _service;

    public MessageController(IMessageService service)
    {
        _service = service;
    }

    private Guid GetUserIdFromClaims()
    {
        var userIdClaim = User.FindFirst("sub")?.Value 
            ?? User.FindFirst("nameid")?.Value 
            ?? User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User ID not found in claims");
        }
        return userId;
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

    [Authorize]
    [HttpPost("")]
    public async Task<IActionResult> CreateMessage([FromBody] MessageCreateDto message)
    {
        var senderId = GetUserIdFromClaims();
        var dto = message with { SenderId = senderId };
        var created = await _service.CreateAsync(dto);
        return Ok(created);
    }

    [Authorize]
    [HttpPost("voice")]
    public async Task<IActionResult> CreateVoiceMessage([FromForm] VoiceMessageCreateRequest request)
    {
        if (request?.File == null) 
            return BadRequest("File is required");

        var file = request.File;
        var senderId = GetUserIdFromClaims();

        await using var ms = new MemoryStream();
        await file.CopyToAsync(ms);
        ms.Seek(0, SeekOrigin.Begin);

        var dto = new VoiceMessageCreateDto
        {
            DialogId = request.DialogId,
            SenderId = senderId,
            AudioStream = ms,
            FileName = file.FileName,
            ContentType = file.ContentType
        };

        var transcription = await _service.CreateVoiceAsync(dto);
        return Ok(new { Transcription = transcription });
    }

    [Authorize]
    [HttpPost("ai-reply")]
    public async Task<IActionResult> GetAiReply([FromBody] AiReplyRequest request)
    {

        if (request.DialogId == Guid.Empty)
            return BadRequest("Dialog ID is required");

        var userId = GetUserIdFromClaims();

        if (request.HistoryLimit <= 0)
            request.HistoryLimit = 10;
        if (request.HistoryLimit > 50)
            request.HistoryLimit = 50;

        var response = await _service.GetAiReplyAsync(userId, request);

        if (!response.Success)
            return BadRequest(response);

        return Ok(response);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMessage([FromBody] MessageCreateDto messageUpdate)
    {
        await _service.UpdateAsync(messageUpdate);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(Guid id)
    {
        var isDeleted = await _service.DeleteAsync(id);
        return Ok(isDeleted);
    }
}
