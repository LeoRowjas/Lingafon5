using Lingafon.Application.DTOs.Auth;

namespace Lingafon.Application.Interfaces.Services;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto loginRequestDto);
    Task<LoginResponseDto> RegisterAsync(RegistrationRequestDto registrationRequestDto);
}