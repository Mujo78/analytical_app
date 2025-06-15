using System;
using server.DTO.Post;

namespace server.Services.IServices;

public interface IPostService
{
    Task CreatePostAsync(CreateDTO post, int userId, bool useDapper = false);
    Task<int> DeletePostAsync(int postId, int userId);
    Task<int> DeletePostWithDapperAsync(int postId, int userId);
    Task<PostDTO> GetPostById(int postId, bool useDapper);
}
