using System;

namespace server.DTO.User;

public class UserAnalyticDTO
{
    public int UserId { get; set; }
    public string DisplayName { get; set; }

    public int PostsCount { get; set; }
    public int CommentsCount { get; set; }
    public double AveragePostScore { get; set; }
    public int TotalViewsOnPosts { get; set; }
    public DateTime? EarliestPostDate { get; set; }
    public DateTime? LatestPostDate { get; set; }
    public int latestPostCreatedId { get; set; }
}
