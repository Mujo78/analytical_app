using System.Data;
using server.Models;

namespace server.Repository.IRepository.IPost;

public interface IPostDapperRepository : IPostRepository
{
    Task<Post?> GetPostByIdAsync(int postId, IDbConnection connection, IDbTransaction transaction);
    Task DeleteAllCommentsByPostIdAsync(int postId, IDbConnection connection, IDbTransaction transaction);
    Task DeletePostAsync(Post post, IDbConnection connection, IDbTransaction transaction);
}
