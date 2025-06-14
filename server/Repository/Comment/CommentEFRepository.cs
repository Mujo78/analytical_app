using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTO.Comment;
using server.Models;
using server.Repository.IRepository.IComment;

namespace server.Repository;

public class CommentEFRepository(EntityDBContext context) : ICommentEFRepository
{
    private readonly EntityDBContext _context = context;

    /// <summary>
    /// Entity Framework Core implementation of ICommentRepository
    /// </summary>
    public async Task DeleteCommentAsync(Comment comment)
    {
        comment.Post.CommentCount--;
        _context.Comments.Remove(comment);
        await _context.SaveChangesAsync();
    }

    public async Task<Comment?> GetCommentByIdAsync(int commentId)
    {
        return await _context.Comments.Include(p => p.Post).FirstOrDefaultAsync(c => c.Id == commentId);
    }

    public async Task<List<CommentDTO>> GetCommentsByPostIdAsync(int postId)
    {
        var comments = await _context.Comments
            .Where(c => c.PostId == postId)
            .OrderByDescending(c => c.CreationDate).Include(c => c.User)
            .Take(10)
            .ToListAsync();

        return [..comments.Select(c => new CommentDTO
        {
            Id = c.Id,
            Text = c.Text,
            CreationDate = c.CreationDate,
            UserId = c.UserId ?? 0,
            UserDisplayName = c.User.DisplayName,
            Score = c.Score ?? 0,
            PostId = c.PostId
        })];
    }

}
