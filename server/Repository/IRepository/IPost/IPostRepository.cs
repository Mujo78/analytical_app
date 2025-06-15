using server.DTO.Post;
using server.Models;

namespace server.Repository.IRepository;

public interface IPostRepository
{
    Task CreatePostAsync(Post post, int userId);
    Task<Post?> GetPostByIdAsync(int postId);
    Task<LastPostDTO?> GetLastPostById(int postId);
    Task DeletePostAsync(Post post);
    Task DeleteAllCommentsByPostIdAsync(int postId);
}
