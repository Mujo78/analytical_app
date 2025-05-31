using System;
using server.DTO.Post;
using server.Services.IServices;

namespace server.Services;

public class PostService : IPostsService
{
    public Task<List<PostDTO>> GetLastPostsAsync(int count = 10, int page = 1)
    {
        throw new NotImplementedException();
    }
}
