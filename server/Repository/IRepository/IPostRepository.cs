using server.Models;

namespace server.Repository.IRepository;

public interface IPostRepository
{
    // Entity Framework Core methods
    Task CreatePostAsync(Post post, int userId);
    Task<Post?> GetPostByIdAsync(int postId);
    Task DeletePostAsync(Post post);
    Task DeleteAllCommentsByPostIdAsync(int postId);
}
