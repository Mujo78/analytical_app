using System.Data;
using server.Models;
namespace server.Repository.IRepository.IUser;

public interface IUserDapperRepository : IUserRepository
{
    Task<List<User>> GetUsersForReputationBonusAsync(IDbConnection connection, IDbTransaction transaction);
    Task UpdateUsersReputationAsync(List<User> users, IDbConnection connection, IDbTransaction transaction);
}
