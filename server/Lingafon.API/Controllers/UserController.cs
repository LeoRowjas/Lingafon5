using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.Interfaces.Services;
using Lingafon.Core.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Lingafon.Core.Interfaces.Repositories;

namespace Lingafon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;
    private readonly IFileStorageService _storageService;
    private readonly ITeacherStudentRepository _teacherStudentRepository;
    public UserController(IUserService service, IFileStorageService storageService, ITeacherStudentRepository teacherStudentRepository)
    {
        _service = service;
        _storageService = storageService;
        _teacherStudentRepository = teacherStudentRepository;
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
    public async Task<IActionResult> GetAll()
    {
        var users = await _service.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMe()
    {
        var id = GetUserIdFromClaims();
        var user = await _service.GetByIdAsync(id);
        return Ok(user);
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
    
    [Authorize]
    [HttpGet("me/students")]
    public async Task<IActionResult> GetStudents()
    {
        var userId = GetUserIdFromClaims();
        var students = await _teacherStudentRepository.GetAllFotTeacherAsync(userId);
        
        return Ok(students);
    }

    [Authorize]
    [HttpGet("me/teachers")]
    public async Task<IActionResult> GetTeachers()
    {
        var userId = GetUserIdFromClaims();
        var teachers = await _teacherStudentRepository.GetAllFotStudentAsync(userId);
        
        return Ok(teachers);
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
    [HttpPost("me/avatar")]
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
    [HttpDelete("me/avatar")]
    public async Task<IActionResult> DeleteAvatar()
    {
        var userId = GetCurrentUserId();
        var isDeleted = await _service.DeleteAvatarAsync(userId);
        return Ok(isDeleted);
    }
    
    [HttpGet("{id}/status")]
    public async Task<IActionResult> GetStatus(Guid id)
    {
        var status = await _service.GetStatusAsync(id);
        return Ok(status);
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
