using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Models;
using server.Repository.IRepository.IPost;

namespace server.Repository;

public class PostEFRepository(EntityDBContext context) : IPostEFRepository
{
    private readonly EntityDBContext _context = context;
    /// <summary>
    /// Entity Framework Core implementation of IPostRepository
    /// <summary>
    public async Task CreatePostAsync(Post post, int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user != null)
        {
            post.LastEditorDisplayName = user!.DisplayName;
        }

        post.Comments =
        [
            new()
            {
                Text = "This is a draft, first comment.",
                CreationDate = DateTime.UtcNow,
                UserId = userId,
                Score = 0,
                PostId = post.Id
            },
            new()
            {
                Text = "This is a draft, second comment.",
                CreationDate = DateTime.UtcNow,
                UserId = userId,
                Score = 0,
                PostId = post.Id
            },
            new()
            {
                Text = "This is a draft, third comment.",
                CreationDate = DateTime.UtcNow,
                UserId = userId,
                Score = 0,
                PostId = post.Id
            },
            new()
            {
                Text = "This is a draft, fourth comment.",
                CreationDate = DateTime.UtcNow,
                UserId = userId,
                Score = 0,
                PostId = post.Id
            }
        ];

        post.CommentCount += post.Comments.Count;

        await _context.Posts.AddAsync(post);
        await _context.SaveChangesAsync();
    }

    public async Task DeletePostAsync(Post post)
    {
        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
    }

    public async Task<Post?> GetPostByIdAsync(int postId)
    {
        return await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId);
    }
    public async Task DeleteAllCommentsByPostIdAsync(int postId)
    {
        var comments = await _context.Comments
              .Where(c => c.PostId == postId)
              .ToListAsync();


        _context.Comments.RemoveRange(comments);
        await _context.SaveChangesAsync();
    }
}
