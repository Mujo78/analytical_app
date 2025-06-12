using System;
using System.Data;
using server.DTO.User;
using server.Models;
using server.Repository.IRepository;
namespace server.Repository.IRepository.IUser;

public interface IUserDapperRepository : IUserRepository
{
    // Entity Framework Core methods
    Task<List<User>> GetUsersForReputationBonusAsync(IDbConnection connection, IDbTransaction transaction);
    Task UpdateUsersReputationAsync(List<User> users, IDbConnection connection, IDbTransaction transaction);
}
