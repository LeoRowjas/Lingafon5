using Lingafon.Core.Entities;
using Lingafon.Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace Lingafon.Infrastructure.Services;

public class PasswordHashService : IPasswordHasher
{
    private PasswordHasher<User> _passwordHasher;

    public PasswordHashService(PasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}