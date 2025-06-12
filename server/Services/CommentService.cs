using System;
using server.DTO.Comment;
using server.Repository.IRepository;
using server.Services.IServices;

namespace server.Services;

public class CommentService(ICommentRepository commentRepository) : ICommentService
{
    private readonly ICommentRepository _commentRepository = commentRepository;

    public async Task<int> DeleteCommentAsync(int commentId, int postId)
    {
        var comment = await _commentRepository.GetCommentByIdAsync(commentId);
        if (comment == null || comment.PostId != postId)
        {
            throw new ArgumentException("Comment not found or does not belong to the specified post.", nameof(commentId));
        }

        var commentToDeleteId = comment.Id;
        await _commentRepository.DeleteCommentAsync(comment);
        return commentToDeleteId;
    }

    public Task<List<CommentDTO>> GetCommentsByPostIdAsync(int postId)
    {
        if (postId <= 0)
        {
            throw new ArgumentException("Post ID must be greater than zero.", nameof(postId));
        }

        return _commentRepository.GetCommentsByPostIdAsync(postId);
    }
}
