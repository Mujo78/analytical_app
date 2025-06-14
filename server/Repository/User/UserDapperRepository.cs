using server.Data;
using server.DTO.User;
using server.Models;
using Dapper;
using server.Repository.IRepository.IUser;
using System.Data;

namespace server.Repository;

public class UserDapperRepository(DapperContext dapperContext) : IUserDapperRepository
{
    private readonly DapperContext _dapperContext = dapperContext;

    /// <summary>
    /// Dapper implementation of IUserRepository
    /// NOTE: Every method which uses Dapper will have word Dapper inside the method name.
    /// </summary>
    public async Task CreateUserAsync(User user)
    {
        var sql = @"
            INSERT INTO Users 
            (AboutMe, Age, CreationDate, DisplayName, DownVotes, EmailHash, LastAccessDate, 
            Location, Reputation, UpVotes, Views, WebsiteUrl, AccountId)
            VALUES 
            (@AboutMe, @Age, @CreationDate, @DisplayName, @DownVotes, @EmailHash, @LastAccessDate,
            @Location, @Reputation, @UpVotes, @Views, @WebsiteUrl, @AccountId);
        ";
        using var connection = _dapperContext.CreateConnection();
        await connection.ExecuteAsync(sql,
            new
            {
                user.AboutMe,
                user.Age,
                CreationDate = DateTime.UtcNow,
                user.DisplayName,
                user.DownVotes,
                user.EmailHash,
                LastAccessDate = DateTime.UtcNow,
                user.Location,
                user.Reputation,
                user.UpVotes,
                user.Views,
                user.WebsiteUrl,
                AccountId = user.AccountId ?? 0
            }
        );
    }

    public async Task<List<User>> GetTopReputationUsersAsync()
    {
        var sql = @"
            SELECT TOP 10 u.*
            FROM Users u
            WHERE u.Reputation > 0
            AND DATEDIFF(DAY, u.CreationDate, u.LastAccessDate) > 0
            AND EXISTS (
                SELECT 1 FROM Posts p WHERE p.OwnerUserId = u.Id
            )
            ORDER BY 
                CAST(u.Reputation AS FLOAT) / DATEDIFF(DAY, u.CreationDate, u.LastAccessDate) DESC;
        ";

        using var connection = _dapperContext.CreateConnection();

        var users = await connection.QueryAsync<User>(sql);

        return [.. users];
    }

    public async Task<UserAnalyticDTO?> GetUserAnalyticsAsync(int userId)
    {
        var sql = @"
            SELECT 
                u.Id AS UserId,
                u.DisplayName,
                COUNT(DISTINCT p.Id) AS PostsCount,
                COUNT(DISTINCT c.Id) AS CommentsCount,
                COALESCE(AVG(CAST(p.Score AS FLOAT)), 0) AS AveragePostScore,
                COALESCE(SUM(p.ViewCount), 0) AS TotalViewsOnPosts,
                MIN(p.CreationDate) AS EarliestPostDate,
                MAX(p.CreationDate) AS LatestPostDate,
                (SELECT TOP 1 p2.Id 
                FROM Posts p2 
                WHERE p2.OwnerUserId = u.Id 
                ORDER BY p2.CreationDate DESC) AS latestPostCreatedId
            FROM Users u
            LEFT JOIN Posts p ON p.OwnerUserId = u.Id
            LEFT JOIN Comments c ON c.UserId = u.Id
            WHERE u.Id = @UserId
            GROUP BY u.Id, u.DisplayName;
        ";

        using var connection = _dapperContext.CreateConnection();

        var result = await connection.QuerySingleOrDefaultAsync<UserAnalyticDTO>(sql, new { UserId = userId });

        return result;
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        var sql = "SELECT * FROM Users WHERE Id = @UserId";

        using var connection = _dapperContext.CreateConnection();
        var user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { UserId = userId });

        return user;
    }

    public async Task<List<User>> GetUsersForReputationBonusAsync(IDbConnection connection, IDbTransaction transaction)
    {
        var sql = @"
        WITH UserPostStats AS (
            SELECT 
                u.Id AS UserId,
                COUNT(p.Id) AS PostsCount,
                COUNT(DISTINCT CASE WHEN c.Id IS NOT NULL THEN p.Id END) AS PostsWithCommentsCount,
                COUNT(c.Id) AS TotalCommentsCount
            FROM Users u
            LEFT JOIN Posts p ON p.OwnerUserId = u.Id
            LEFT JOIN Comments c ON c.PostId = p.Id
            GROUP BY u.Id
        )
        SELECT u.*
        FROM Users u
        JOIN UserPostStats ups ON ups.UserId = u.Id
        WHERE ups.PostsCount > 10
          AND ups.PostsWithCommentsCount >= 10
          AND ups.TotalCommentsCount > 30;
    ";

        var users = await connection.QueryAsync<User>(sql, transaction: transaction);

        return [.. users];
    }

    public async Task UpdateUsersReputationAsync(List<User> users, IDbConnection connection, IDbTransaction transaction)
    {
        var sql = @"
            UPDATE Users
            SET Reputation = Reputation + 50,
                UpVotes = UpVotes + 5
            WHERE Id = @Id
        ";

        foreach (var user in users)
        {
            await connection.ExecuteAsync(sql, new { user.Id }, transaction: transaction);
        }
    }

    public async Task UpdateUserAsync(User user)
    {
        var sql = @"
            UPDATE Users SET
                AboutMe = @AboutMe,
                Age = @Age,
                CreationDate = @CreationDate,
                DisplayName = @DisplayName,
                DownVotes = @DownVotes,
                EmailHash = @EmailHash,
                LastAccessDate = @LastAccessDate,
                Location = @Location,
                Reputation = @Reputation,
                UpVotes = @UpVotes,
                Views = @Views,
                WebsiteUrl = @WebsiteUrl,
                AccountId = @AccountId
            WHERE Id = @Id;
        ";

        using var connection = _dapperContext.CreateConnection();
        await connection.ExecuteAsync(sql, new
        {
            user.AboutMe,
            user.Age,
            user.CreationDate,
            user.DisplayName,
            user.DownVotes,
            user.EmailHash,
            user.LastAccessDate,
            user.Location,
            user.Reputation,
            user.UpVotes,
            user.Views,
            user.WebsiteUrl,
            user.AccountId,
            user.Id
        });
    }
}
