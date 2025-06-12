using server.Models;

namespace server.Repository.IRepository.IUser;

public interface IUserEFRepository : IUserRepository
{
    // Entity Framework Core methods
    Task<List<User>> GetUsersForReputationBonusAsync();
}
