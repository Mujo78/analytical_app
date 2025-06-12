using System;
using server.DTO.Comment;

namespace server.Services.IServices;

public interface ICommentService
{
    Task<List<CommentDTO>> GetCommentsByPostIdAsync(int postId);
    Task<int> DeleteCommentAsync(int commentId, int userId);
}
