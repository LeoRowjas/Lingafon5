using Lingafon.Core.Enums;

namespace Lingafon.Core.Interfaces.Services;

public interface IJwtTokenService
{
    string GenerateJwtToken(Guid userId, string email, UserRole role);
}