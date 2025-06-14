using System;
using server.DTO.Comment;

namespace server.Services.IServices;

public interface ICommentService
{
    Task<List<CommentDTO>> GetCommentsByPostIdAsync(int postId, bool useDapper = false);
    Task<int> DeleteCommentAsync(int commentId, int post);
    Task<int> DeleteCommentWithDapperAsync(int commentId, int postId);
}
