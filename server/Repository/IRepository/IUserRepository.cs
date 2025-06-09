using System;
using server.Models;

namespace server.Repository.IRepository;

public interface IUserRepository
{
    Task<List<User>> GetTopReputationUsersAsync();
}
