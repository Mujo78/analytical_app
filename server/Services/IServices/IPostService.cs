using System;
using server.DTO.Post;

namespace server.Services.IServices;

public interface IPostService
{
    Task CreatePostAsync(CreateDTO post, int userId);
    Task<int> DeletePostAsync(int postId, int userId);
}
