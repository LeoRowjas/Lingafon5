using Lingafon.Application.DTOs.Auth;
using Lingafon.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace Lingafon.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequestDto request)
    {
        var response = await _authService.LoginAsync(request);
        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegistrationRequestDto request)
    {
        var response = await _authService.RegisterAsync(request);
        return Ok(response);
    }
}