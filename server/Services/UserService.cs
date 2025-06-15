using System;
using AutoMapper;
using server.Data;
using server.DTO.User;
using server.Models;
using server.Repository;
using server.Repository.IRepository;
using server.Repository.IRepository.IUser;
using server.Services.IServices;
using EFCore.BulkExtensions;

namespace server.Services;

public class UserService(IUserEFRepository userEFRepository, IUserDapperRepository userDapperRepository, IMapper mapper, EntityDBContext context, DapperContext dapperContext) : IUserService
{
    private readonly IUserEFRepository _userEFRepository = userEFRepository;
    private readonly IUserDapperRepository _userDapperRepository = userDapperRepository;
    private readonly EntityDBContext _context = context;
    private readonly DapperContext _dapperContext = dapperContext;
    private readonly IMapper _mapper = mapper;

    public async Task CreateUserAsync(CreateUserDTO user, bool useDapper = false)
    {
        if (user.DisplayName == null || user.DisplayName.Trim().Length == 0)
        {
            throw new ArgumentException("Name cannot be null or empty.", nameof(user.DisplayName));
        }

        User userToCreate = new()
        {
            DisplayName = user.DisplayName,
            EmailHash = user.Email.GetHashCode().ToString(),
            Location = user.Location,
            AboutMe = user.AboutMe,
            Age = user.Age,
            WebsiteUrl = user.WebsiteUrl,
            Views = 0,
            CreationDate = DateTime.UtcNow,
            LastAccessDate = DateTime.UtcNow,
            Reputation = 0,
            DownVotes = 0,
            UpVotes = 0
        };

        try
        {
            if (useDapper)
            {
                await _userDapperRepository.CreateUserAsync(userToCreate);
            }
            else
            {
                await _userEFRepository.CreateUserAsync(userToCreate);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating the user. Please try again later. " + ex.Message);
        }
    }

    public async Task DistributeBonusReputationAsync()
    {
        var transaction = await _context.Database.BeginTransactionAsync();
        var users = await _userEFRepository.GetUsersForReputationBonusAsync() ?? throw new Exception("No users found for reputation bonus distribution.");
        try
        {

            foreach (var user in users)
            {
                user.Reputation += 50;
                user.UpVotes += 5;
            }

            await _context.BulkUpdateAsync(users);
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw new Exception("An error occurred while distributing bonus reputation. Please try again later.");
        }
    }

    public async Task<List<TopUserReputationDTO>> GetTopReputationUsersAsync(bool useDapper = false)
    {
        if (useDapper)
        {
            return await _userDapperRepository.GetTopReputationUsersAsync();
        }
        else
        {
            return await _userEFRepository.GetTopReputationUsersAsync();
        }
    }

    public async Task<UserDTO> GetUserByIdAsync(int userId, bool useDapper = false)
    {
        if (userId <= 0)
        {
            throw new ArgumentException("User ID must be greater than zero.", nameof(userId));
        }

        User user;
        if (useDapper)
        {
            user = await _userDapperRepository.GetUserByIdAsync(userId) ?? throw new KeyNotFoundException($"User with ID {userId} not found."); ;
        }
        else
        {
            user = await _userEFRepository.GetUserByIdAsync(userId) ?? throw new KeyNotFoundException($"User with ID {userId} not found."); ;
        }

        return _mapper.Map<UserDTO>(user);
    }

    public async Task<UserAnalyticDTO> GetUsersAnalyticsAsync(int userId, bool useDapper = false)
    {
        UserAnalyticDTO userAnalytics;
        if (useDapper)
        {
            userAnalytics = await _userDapperRepository.GetUserAnalyticsAsync(userId) ?? throw new KeyNotFoundException($"User with ID {userId} not found."); ;
        }
        else
        {
            userAnalytics = await _userEFRepository.GetUserAnalyticsAsync(userId) ?? throw new KeyNotFoundException($"User with ID {userId} not found."); ;
        }

        return userAnalytics;
    }

    public async Task<UserDTO> UpdateUserAsync(UpdateUserDTO userUpdateDTO, int userId, bool useDapper = false)
    {
        User user;

        if (useDapper)
        {
            user = await _userDapperRepository.GetUserByIdAsync(userId) ?? throw new KeyNotFoundException($"User with ID {userId} not found.");
        }
        else
        {
            user = await _userEFRepository.GetUserByIdAsync(userId) ?? throw new KeyNotFoundException($"User with ID {userId} not found.");
        }

        try
        {
            user.EmailHash = userUpdateDTO.Email.GetHashCode().ToString();
            user.DisplayName = userUpdateDTO.DisplayName;
            user.Location = userUpdateDTO.Location;
            user.AboutMe = userUpdateDTO.AboutMe;
            user.Age = userUpdateDTO.Age;
            user.WebsiteUrl = userUpdateDTO.WebsiteUrl;
            user.LastAccessDate = DateTime.UtcNow;

            if (useDapper)
            {
                await _userDapperRepository.UpdateUserAsync(user);
            }
            else
            {
                await _userEFRepository.UpdateUserAsync(user);
            }
            return _mapper.Map<UserDTO>(user);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while updating the user. Please try again later. " + ex.Message);
        }
    }

    /// <summary>
    /// Dapper usage
    /// </summary>

    public async Task DistributeBonusReputationWithDapperAsync()
    {
        using var connection = _dapperContext.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            var users = await _userDapperRepository.GetUsersForReputationBonusAsync(connection, transaction) ?? throw new Exception("No users found for reputation bonus distribution.");
            await _userDapperRepository.UpdateUsersReputationAsync(users, connection, transaction);

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception("An error occurred while distributing bonus reputation. Please try again later. " + ex.Message);
        }
    }

}
