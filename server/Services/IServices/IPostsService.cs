using System;
using server.DTO.Post;

namespace server.Services.IServices;

public interface IPostsService
{
    Task<List<PostDTO>> GetLastPostsAsync(int count = 10, int page = 1);
}
