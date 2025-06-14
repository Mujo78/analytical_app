using server.Data;
using server.Models;
using server.Repository.IRepository.IUser;
using Microsoft.EntityFrameworkCore;
using server.DTO.User;

namespace server.Repository;

public class UserEFRepository(EntityDBContext context) : IUserEFRepository
{
    private readonly EntityDBContext _context = context;
    /// <summary>
    /// Entity Framework Core implementation of IUserRepository
    /// </summary>
    public async Task CreateUserAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<List<User>> GetTopReputationUsersAsync()
    {
        return await _context.Users
                .Where(u => u.Reputation > 0 &&
                        EF.Functions.DateDiffDay(u.CreationDate, u.LastAccessDate) > 0 && u.Posts.Any())
                .OrderByDescending(u =>
                        (double)u.Reputation / EF.Functions.DateDiffDay(u.CreationDate, u.LastAccessDate))
                .Take(10)
                .ToListAsync();
    }

    public async Task<UserAnalyticDTO?> GetUserAnalyticsAsync(int userId)
    {
        var userAnalytics = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new
            {
                UserId = u.Id,
                u.DisplayName,
                PostsCount = u.Posts.Count,
                CommentsCount = u.Comments.Count,
                AveragePostScore = u.Posts.Select(p => (double?)p.Score).Average() ?? 0,
                TotalViewsOnPosts = u.Posts.Select(p => (int?)p.ViewCount).Sum() ?? 0,
                EarliestPostDate = u.Posts.Select(p => (DateTime?)p.CreationDate).Min(),
                LatestPostDate = u.Posts.Select(p => (DateTime?)p.CreationDate).Max(),
                latestPostCreatedId = u.Posts.OrderByDescending(p => p.CreationDate).Select(p => p.Id).FirstOrDefault()
            })
            .FirstOrDefaultAsync();

        if (userAnalytics != null)
        {
            return new UserAnalyticDTO()
            {
                UserId = userAnalytics.UserId,
                DisplayName = userAnalytics.DisplayName,
                PostsCount = userAnalytics.PostsCount,
                CommentsCount = userAnalytics.CommentsCount,
                AveragePostScore = userAnalytics.AveragePostScore,
                TotalViewsOnPosts = userAnalytics.TotalViewsOnPosts,
                EarliestPostDate = userAnalytics.EarliestPostDate,
                LatestPostDate = userAnalytics.LatestPostDate,
                latestPostCreatedId = userAnalytics.latestPostCreatedId
            };
        }
        return null;
    }

    public Task<User?> GetUserByIdAsync(int userId)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<List<User>> GetUsersForReputationBonusAsync()
    {
        var eligibleUsers = await _context.Users
            .Where(u => u.Posts.Count() > 10)
            .Select(u => new
            {
                User = u,
                PostsWithComments = u.Posts.Count(p => p.Comments.Any()),
                TotalComments = u.Posts.SelectMany(p => p.Comments).Count()
            })
            .Where(x => x.PostsWithComments >= 10 && x.TotalComments > 30)
            .Select(x => x.User)
            .ToListAsync();

        return eligibleUsers;
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
