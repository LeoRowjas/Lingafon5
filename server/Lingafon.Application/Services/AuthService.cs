using Lingafon.Application.DTOs.Auth;
using Lingafon.Application.Interfaces.Services;
using Lingafon.Core.Entities;
using Lingafon.Core.Interfaces.Repositories;
using Lingafon.Core.Interfaces.Services;

namespace Lingafon.Application.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtService;
    private readonly IPasswordHasher _passwordHasher;

    public AuthService(IUserRepository userRepository, IJwtTokenService jwtService, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email cannot be empty", nameof(request.Email));
        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password cannot be empty", nameof(request.Password));

        var user = await _userRepository.GetByEmailAsync(request.Email);
        if (user == null || !_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password))
        {
            throw new UnauthorizedAccessException("Invalid email or password");
        }
        
        var token = _jwtService.GenerateJwtToken(user.Id, user.Email, user.Role);
        var response = new LoginResponseDto()
        {
            Email = user.Email,
            Token = token,
            UserId = user.Id
        };
        return response;
    }

    public async Task<LoginResponseDto> RegisterAsync(RegistrationRequestDto request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));
        if (string.IsNullOrWhiteSpace(request.Email))
            throw new ArgumentException("Email cannot be empty", nameof(request.Email));
        if (string.IsNullOrWhiteSpace(request.Password))
            throw new ArgumentException("Password cannot be empty", nameof(request.Password));
        if (string.IsNullOrWhiteSpace(request.FirstName))
            throw new ArgumentException("FirstName cannot be empty", nameof(request.FirstName));
        if (string.IsNullOrWhiteSpace(request.LastName))
            throw new ArgumentException("LastName cannot be empty", nameof(request.LastName));

        var existingUser = await _userRepository.GetByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new InvalidOperationException("User with this email already exists");
        }
        
        var user = new User()
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            MiddleName = request.MiddleName,
            Role = request.Role,
            CreatedAt = DateTime.UtcNow,
        };
        user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);
        
        await _userRepository.AddAsync(user);
        var token = _jwtService.GenerateJwtToken(user.Id, user.Email, user.Role);

        return new LoginResponseDto()
        {
            UserId = user.Id,
            Email = user.Email,
            Token = token
        };
    }
}