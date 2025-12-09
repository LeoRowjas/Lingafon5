using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.Interfaces.Services;
using Lingafon.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Lingafon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    private readonly IFileStorageService _storageService;
    public UserController(IUserService service, IFileStorageService storageService)
    {
        _service = service;
        _storageService = storageService;
    }
    
    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        var users = await _service.GetAllAsync();
        return Ok(users);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var user = await _service.GetByIdAsync(id);
        return Ok(user);
    }
    
    [HttpGet("by-email")]
    public async Task<IActionResult> GetByEmail([FromQuery] string email)
    {
        var user = await _service.GetByEmailAsync(email);
        return Ok(user);
    }
    
    [HttpPost("")]
    public async Task<IActionResult> CreateUser([FromBody] UserCreateDto user)
    {
        var created = await _service.CreateAsync(user);
        return Ok(created);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto userUpdate)
    {
        await _service.UpdateAsync(userUpdate);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var isDeleted = await _service.DeleteAsync(id);
        return Ok(isDeleted);
    }

    [Authorize]
    [HttpPost("avatar")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadAvatar(IFormFile file)
    {
        var userId = GetCurrentUserId();
        
        if (file == null || file.Length == 0)
            return BadRequest("No file provided");
        
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{userId}{extension}";

        await using var stream = file.OpenReadStream();
        var avatarUrl = await _service.UpdateAvatarUrlAsync(userId, stream, fileName,  file.ContentType);
        
        return Ok(new {AvatarUrl = avatarUrl});
    }

    [Authorize]
    [HttpDelete("avatar")]
    public async Task<IActionResult> DeleteAvatar()
    {
        var userId = GetCurrentUserId();
        var isDeleted = await _service.DeleteAvatarAsync(userId);
        return Ok(isDeleted);
    }
    
    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("Invalid user token");
        }
        return userId;
    }
}

