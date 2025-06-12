using System;
using server.DTO.Comment;
using server.Models;

namespace server.Repository.IRepository;

public interface ICommentRepository
{
    Task<List<CommentDTO>> GetCommentsByPostIdAsync(int postId);
    Task<Comment?> GetCommentByIdAsync(int commentId);
    Task DeleteCommentAsync(Comment comment);
}
