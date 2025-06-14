using server.DTO.User;

namespace server.Services.IServices;

public interface IUserService
{
    Task<List<UserDTO>> GetTopReputationUsersAsync(bool useDapper = false);
    Task<UserAnalyticDTO> GetUsersAnalyticsAsync(int userId, bool useDapper = false);
    Task CreateUserAsync(CreateUserDTO user, bool useDapper = false);
    Task<UserDTO> UpdateUserAsync(UpdateUserDTO user, int userId, bool useDapper = false);
    Task<UserDTO> GetUserByIdAsync(int userId, bool useDapper = false);
    Task DistributeBonusReputationAsync();
    Task DistributeBonusReputationWithDapperAsync();

}
