using Lingafon.Application.DTOs.FromEntities;
using Lingafon.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lingafon.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser([FromBody] UserUpdateDto userUpdate)
    {
        await _service.UpdateAsync(userUpdate);
        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var isDeleted = await _service.DeleteAsync(id);
        return Ok(isDeleted);
    }
}

