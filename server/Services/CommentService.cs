using server.DTO.Comment;
using server.Repository.IRepository.IComment;
using server.Services.IServices;

namespace server.Services;

public class CommentService(ICommentEFRepository commentEFRepository, ICommentDapperRepository commentDapperRepository) : ICommentService
{
    private readonly ICommentEFRepository commentEFRepository = commentEFRepository;
    private readonly ICommentDapperRepository commentDapperRepository = commentDapperRepository;

    public async Task<int> DeleteCommentAsync(int commentId, int postId)
    {
        var comment = await commentEFRepository.GetCommentByIdAsync(commentId);
        if (comment == null || comment.PostId != postId)
        {
            throw new ArgumentException("Comment not found or does not belong to the specified post.", nameof(commentId));
        }

        var commentToDeleteId = comment.Id;
        await commentEFRepository.DeleteCommentAsync(comment);
        return commentToDeleteId;
    }

    public async Task<int> DeleteCommentWithDapperAsync(int commentId, int postId)
    {
        var comment = await commentDapperRepository.GetCommentByIdAsync(commentId);
        if (comment == null || comment.PostId != postId)
        {
            throw new ArgumentException("Comment not found or does not belong to the specified post.", nameof(commentId));
        }

        var commentToDeleteId = comment.Id;
        await commentDapperRepository.DeleteCommentAsync(comment);
        return commentToDeleteId;
    }

    public async Task<List<CommentDTO>> GetCommentsByPostIdAsync(int postId, bool useDapper)
    {
        if (postId <= 0)
        {
            throw new ArgumentException("Post ID must be greater than zero.", nameof(postId));
        }

        if (useDapper)
        {
            return await commentDapperRepository.GetCommentsByPostIdAsync(postId);
        }
        else
        {
            return await commentEFRepository.GetCommentsByPostIdAsync(postId);
        }
    }
}
