using System;
using server.Data;
using server.Repository.IRepository;
using Dapper;
using server.Models;

namespace server.Repository;

public class PostDapperRepository(DapperContext dapperContext) : IPostRepository
{
    private readonly DapperContext _dapperContext = dapperContext;
    /// <summary>
    /// Dapper implementation of IPostRepository
    /// </summary>
    public async Task CreatePostAsync(Post post, int userId)
    {
        const string getUserSql = "SELECT DisplayName FROM Users WHERE Id = @UserId;";

        const string insertPostSql = @"
            INSERT INTO Posts 
            (Body, CreationDate, LastEditorDisplayName, OwnerUserId, CommentCount, PostTypeId, Score, Title, ViewCount)
            OUTPUT INSERTED.Id
            VALUES 
            (@Body, @CreationDate, @LastEditorDisplayName, @OwnerUserId, @CommentCount, @PostTypeId, @Score, @Title, @ViewCount);
        ";

        const string insertCommentSql = @"
            INSERT INTO Comments 
            (Text, CreationDate, UserId, Score, PostId)
            VALUES 
            (@Text, @CreationDate, @UserId, @Score, @PostId);
        ";

        using var connection = _dapperContext.CreateConnection();
        using var transaction = connection.BeginTransaction();

        try
        {
            // Get DisplayName
            var displayName = await connection.QuerySingleOrDefaultAsync<string>(
                getUserSql, new { UserId = userId }, transaction);

            post.LastEditorDisplayName = displayName;

            // Insert Post
            post.CreationDate = DateTime.UtcNow;
            post.CommentCount = 4;

            var postId = await connection.ExecuteScalarAsync<int>(
                insertPostSql,
                new
                {
                    post.Body,
                    post.CreationDate,
                    post.LastEditorDisplayName,
                    OwnerUserId = userId,
                    post.CommentCount,
                    post.PostTypeId,
                    post.Score,
                    post.Title,
                    post.ViewCount
                },
                transaction
            );

            // Insert Comments
            var comments = new[]
            {
            "This is a draft, first comment.",
            "This is a draft, second comment.",
            "This is a draft, third comment.",
            "This is a draft, fourth comment."
        };

            foreach (var text in comments)
            {
                await connection.ExecuteAsync(
                    insertCommentSql,
                    new
                    {
                        Text = text,
                        CreationDate = DateTime.UtcNow,
                        UserId = userId,
                        Score = 0,
                        PostId = postId
                    },
                    transaction
                );
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task DeletePostAsync(Post post)
    {
        const string sql = @"
            DELETE FROM Posts
            WHERE Id = @PostId;
        ";

        using var connection = _dapperContext.CreateConnection();
        await connection.ExecuteAsync(sql, new { PostId = post.Id });
    }

    public async Task<Post?> GetPostByIdAsync(int postId)
    {
        const string sql = @"
            SELECT *
            FROM Posts
            WHERE Id = @PostId;
        ";

        using var connection = _dapperContext.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<Post>(sql, new { PostId = postId });
    }

    public async Task DeleteAllCommentsByPostIdAsync(int postId)
    {
        const string sql = @"
            DELETE FROM Comments
            WHERE PostId = @PostId;
        ";

        using var connection = _dapperContext.CreateConnection();
        await connection.ExecuteAsync(sql, new { PostId = postId });
    }
}
