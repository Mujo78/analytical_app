using System;
using Microsoft.EntityFrameworkCore;
using server.Data;
using server.DTO.Post;
using server.Models;
using server.Repository.IRepository;
using server.Services.IServices;

namespace server.Services;

public class PostService(IPostRepository repository, EntityDBContext dbContext) : IPostService
{
    private readonly IPostRepository _repository = repository;
    private readonly EntityDBContext _dbContext = dbContext;

    public async Task CreatePostAsync(CreateDTO post, int userId)
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
            await _repository.CreatePostAsync(postToCreate, userId);
        }
        catch (Exception)
        {
            throw new Exception("An error occurred while creating the user. Please try again later.");
        }
    }

    public async Task<int> DeletePostAsync(int postId, int userId)
    {
        if (postId <= 0)
        {
            throw new ArgumentException("Post ID must be greater than zero.", nameof(postId));
        }

        var post = await _repository.GetPostByIdAsync(postId) ?? throw new ArgumentException("Post not found.", nameof(postId)); ;
        if (post.OwnerUserId != userId)
        {
            throw new UnauthorizedAccessException("You do not have permission to delete this post.");
        }

        using var transaction = _dbContext.Database.BeginTransaction();

        try
        {
            await _repository.DeleteAllCommentsByPostIdAsync(post.Id);
            await _repository.DeletePostAsync(post);
            await transaction.CommitAsync();
            return post.Id;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            throw new Exception("Something went wrong while deleting the post.", ex);
        }
    }
}
