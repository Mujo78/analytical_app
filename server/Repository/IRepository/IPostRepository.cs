using System;
using server.DTO.Post;

namespace server.Repository.IRepository;

public interface IPostRepository
{
    Task<List<PostDTO>> GetLastPostsAsync(int limit = 10, int page = 1);
}
