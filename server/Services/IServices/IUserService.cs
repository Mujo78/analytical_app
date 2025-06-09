using System;
using server.DTO.User;

namespace server.Services.IServices;

public interface IUserService
{
    Task<List<UserDTO>> GetTopReputationUsersAsync();
}
