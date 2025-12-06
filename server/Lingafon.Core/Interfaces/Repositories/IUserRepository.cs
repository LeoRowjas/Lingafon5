using Lingafon.Core.Entities;

namespace Lingafon.Core.Interfaces.Repositories;

public interface IUserRepository : IRepository<User, Guid>
{
    Task<User?> GetByEmailAsync(string email);
    
}