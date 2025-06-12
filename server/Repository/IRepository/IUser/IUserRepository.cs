using System;
using server.DTO.User;
using server.Models;

namespace server.Repository.IRepository.IUser;

public interface IUserRepository
{
    Task<List<User>> GetTopReputationUsersAsync();
    Task<UserAnalyticDTO?> GetUserAnalyticsAsync(int userId);
    Task CreateUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task<User?> GetUserByIdAsync(int userId);
}
