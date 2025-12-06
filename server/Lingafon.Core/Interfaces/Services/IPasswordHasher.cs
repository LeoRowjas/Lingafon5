using Lingafon.Core.Entities;

namespace Lingafon.Core.Interfaces.Services;

public interface IPasswordHasher
{
    public string HashPassword(User user, string password);
    public bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
}