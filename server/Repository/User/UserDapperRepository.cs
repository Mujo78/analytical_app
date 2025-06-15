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
                user.AccountId
            }
        );
    }

    public async Task<List<TopUserReputationDTO>> GetTopReputationUsersAsync()
    {
        var sql = @"
            SELECT TOP 10 
                u.Id,
                u.DisplayName,
                u.Reputation,
                u.Views,
                u.UpVotes,
                u.DownVotes,
                u.CreationDate
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

        var users = await connection.QueryAsync<TopUserReputationDTO>(sql);

        return [.. users];
    }

    public async Task<UserAnalyticDTO?> GetUserAnalyticsAsync(int userId)
    {
        var sql = @"
        WITH PostAgg AS (
            SELECT 
                OwnerUserId,
                COUNT(*) AS PostsCount,
                AVG(CAST(Score AS FLOAT)) AS AveragePostScore,
                SUM(ViewCount) AS TotalViewsOnPosts,
                MIN(CreationDate) AS EarliestPostDate,
                MAX(CreationDate) AS LatestPostDate
            FROM Posts
            WHERE OwnerUserId = @UserId
            GROUP BY OwnerUserId
        ),
        CommentAgg AS (
            SELECT 
                UserId,
                COUNT(*) AS CommentsCount
            FROM Comments
            WHERE UserId = @UserId
            GROUP BY UserId
        ),
        LatestPost AS (
            SELECT TOP 1 Id
            FROM Posts
            WHERE OwnerUserId = @UserId
            ORDER BY CreationDate DESC
        )
        SELECT 
            u.Id AS UserId,
            u.DisplayName,
            COALESCE(p.PostsCount, 0) AS PostsCount,
            COALESCE(c.CommentsCount, 0) AS CommentsCount,
            COALESCE(p.AveragePostScore, 0) AS AveragePostScore,
            COALESCE(p.TotalViewsOnPosts, 0) AS TotalViewsOnPosts,
            p.EarliestPostDate,
            p.LatestPostDate,
            l.Id AS latestPostCreatedId
        FROM Users u
        LEFT JOIN PostAgg p ON p.OwnerUserId = u.Id
        LEFT JOIN CommentAgg c ON c.UserId = u.Id
        LEFT JOIN LatestPost l ON 1 = 1
        WHERE u.Id = @UserId;";

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
        const int batchSize = 1000;

        var batches = users.Select((u, i) => new { u, i })
                           .GroupBy(x => x.i / batchSize, x => x.u);

        var sql = @"
        UPDATE Users
        SET Reputation = Reputation + 50,
            UpVotes = UpVotes + 5
        WHERE Id IN @Ids
    ";

        foreach (var batch in batches)
        {
            var ids = batch.Select(u => u.Id).ToList();
            await connection.ExecuteAsync(sql, new { Ids = ids }, transaction);
        }
    }

    public async Task UpdateUserAsync(User user)
    {
        var sql = @"
            UPDATE Users SET
            DisplayName = @DisplayName,
            EmailHash = @EmailHash,
            Location = @Location,
            AboutMe = @AboutMe,
            Age = @Age,
            WebsiteUrl = @WebsiteUrl
        WHERE Id = @Id;
        ";

        using var connection = _dapperContext.CreateConnection();
        await connection.ExecuteAsync(sql, new
        {
            user.AboutMe,
            user.Age,
            user.DisplayName,
            user.EmailHash,
            user.Location,
            user.WebsiteUrl,
            user.Id
        });
    }
}
