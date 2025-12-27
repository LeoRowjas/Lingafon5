using System.Security.Claims;

using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lingafon.API.Controllers;

[ApiController]
[Route("[controller]")]
public class InviteController : ControllerBase
{
    private readonly IInviteLinkService _inviteService;

    public InviteController(IInviteLinkService inviteService)
    {
        _inviteService = inviteService;
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
    
    [Authorize]
    [HttpPost("")]
    public async Task<IActionResult> CreateInviteLink([FromBody] InviteLinkCreateDto inviteLinkCreateDto)
    {
        var invite = await _inviteService.CreateAsync(inviteLinkCreateDto);
        return Ok(invite);
    }

    [Authorize]
    [HttpGet("accept")]
    public async Task<IActionResult> AcceptInviteLink([FromQuery]string token)
    {
        var userId = GetUserIdFromClaims();
        var acceptance = await _inviteService.AcceptInviteLink(userId, token);
        return Ok(acceptance);
    }

    [HttpGet("")]
    public async Task<IActionResult> GetAllInviteLink()
    {
        var invites = await _inviteService.GetAllAsync();
        return Ok(invites);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInviteLink(Guid id)
    {
        var invite = await _inviteService.DeleteAsync(id);
        return Ok(invite);
    }
}