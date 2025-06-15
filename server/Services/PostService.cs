using AutoMapper;
using server.Data;
using server.DTO.Post;
using server.Models;
using server.Repository.IRepository.IPost;
using server.Services.IServices;

namespace server.Services;

public class PostService(IMapper mapper, IPostEFRepository postEFRepository, IPostDapperRepository postDapperRepository, EntityDBContext dbContext, DapperContext dapperContext) : IPostService
{
    private readonly IPostEFRepository postEFRepository = postEFRepository;
    private readonly IPostDapperRepository postDapperRepository = postDapperRepository;
    private readonly EntityDBContext _dbContext = dbContext;
    private readonly DapperContext dapperContext = dapperContext;
    private readonly IMapper _mapper = mapper;
    public async Task CreatePostAsync(CreateDTO post, int userId, bool useDapper = false)
    {
        if (post == null)
        {
            throw new ArgumentNullException(nameof(post), "Post cannot be null.");
        }

        Post postToCreate = new()
        {
            Title = post.Title,
            Body = post.Body,
            PostTypeId = post.PostTypeId,
            AnswerCount = 0,
            ViewCount = 0,
            Score = 0,
            FavoriteCount = 0,
            CommentCount = 0,
            LastActivityDate = DateTime.UtcNow,
            OwnerUserId = userId,
            LastEditorUserId = userId,
            CreationDate = DateTime.UtcNow,
            LastEditDate = DateTime.UtcNow,
            Tags = post.Tags,
        };

        try
        {
            if (useDapper)
            {
                await postDapperRepository.CreatePostAsync(postToCreate, userId);
            }
            else
            {
                await postEFRepository.CreatePostAsync(postToCreate, userId);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while creating the user. Please try again later. " + ex.Message);
        }
    }

    public async Task<int> DeletePostAsync(int postId, int userId)
    {
        if (postId <= 0)
        {
            throw new ArgumentException("Post ID must be greater than zero.", nameof(postId));
        }

        var post = await postEFRepository.GetPostByIdAsync(postId) ?? throw new ArgumentException("Post not found.", nameof(postId)); ;
        if (post.OwnerUserId != userId)
        {
            throw new UnauthorizedAccessException("You do not have permission to delete this post.");
        }

        using var transaction = _dbContext.Database.BeginTransaction();

        try
        {
            await postEFRepository.DeleteAllCommentsByPostIdAsync(post.Id);
            await postEFRepository.DeletePostAsync(post);
            await transaction.CommitAsync();
            return post.Id;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception("Something went wrong while deleting the post.", ex);
        }
    }

    public async Task<PostDTO> GetPostById(int postId, bool useDapper)
    {
        if (postId <= 0)
        {
            throw new ArgumentException("Post ID must be greateer than zero");
        }

        Post? post;
        if (useDapper)
        {
            post = await postDapperRepository.GetPostByIdAsync(postId);
        }
        else
        {
            post = await postEFRepository.GetPostByIdAsync(postId);
        }

        if (post == null)
        {
            throw new Exception("Post not found!");
        }
        return _mapper.Map<PostDTO>(post);

    }
    /// <summary>
    ///  Dapper Implementation
    /// </summary>
    public async Task<int> DeletePostWithDapperAsync(int postId, int userId)
    {
        if (postId <= 0)
        {
            throw new ArgumentException("Post ID must be greater than zero.", nameof(postId));
        }

        using var connection = dapperContext.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();


        try
        {
            var post = await postDapperRepository.GetPostByIdAsync(postId, connection, transaction) ?? throw new ArgumentException("Post not found.", nameof(postId));
            if (post.OwnerUserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to delete this post.");
            }
            await postDapperRepository.DeleteAllCommentsByPostIdAsync(post.Id, connection, transaction);
            await postDapperRepository.DeletePostAsync(post, connection, transaction);
            transaction.Commit();
            return post.Id;
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            throw new Exception("Something went wrong while deleting the post. " + ex.Message);
        }
    }
}
