using server.Data;
using Dapper;
using server.Models;
using server.Repository.IRepository.IPost;
using System.Data;
using server.DTO.Post;
using server.DTO.Comment;

namespace server.Repository;

public class PostDapperRepository(DapperContext dapperContext) : IPostDapperRepository
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
            (Body, CreationDate, LastEditorDisplayName, OwnerUserId, CommentCount, PostTypeId, Score, Title, ViewCount, AnswerCount, 
                FavoriteCount, LastActivityDate, LastEditorUserId, LastEditDate, Tags)
            OUTPUT INSERTED.Id
            VALUES 
            (@Body, @CreationDate, @LastEditorDisplayName, @OwnerUserId, @CommentCount, @PostTypeId, @Score, @Title, @ViewCount, 
                @AnswerCount, @FavoriteCount, @LastActivityDate, @LastEditorUserId, @LastEditDate, @Tags);
        ";

        const string insertCommentSql = @"
            INSERT INTO Comments 
            (Text, CreationDate, UserId, Score, PostId)
            VALUES 
            (@Text, @CreationDate, @UserId, @Score, @PostId);
        ";

        using var connection = _dapperContext.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            var displayName = await connection.QuerySingleOrDefaultAsync<string>(
                getUserSql, new { UserId = userId }, transaction);

            post.LastEditorDisplayName = displayName;

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
                    post.ViewCount,
                    post.AnswerCount,
                    post.FavoriteCount,
                    post.LastActivityDate,
                    post.LastEditorUserId,
                    post.LastEditDate,
                    post.Tags
                },
                transaction
            );

            var comments = new[]
            {
            "This is a draft, first comment.",
            "This is a draft, second comment.",
            "This is a draft, third comment.",
            "This is a draft, fourth comment."
        };

            var commentsToInsert = comments.Select(text => new
            {
                Text = text,
                CreationDate = DateTime.UtcNow,
                UserId = userId,
                Score = 0,
                PostId = postId
            }).ToList();

            await connection.ExecuteAsync(
                insertCommentSql,
                commentsToInsert,
                transaction
            );
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

    public async Task DeletePostAsync(Post post, IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"
            DELETE FROM Posts
            WHERE Id = @PostId;
        ";

        await connection.ExecuteAsync(sql, new { PostId = post.Id }, transaction);
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

    public async Task<Post?> GetPostByIdAsync(int postId, IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"
            SELECT *
            FROM Posts
            WHERE Id = @PostId;
        ";

        return await connection.QueryFirstOrDefaultAsync<Post>(sql, new { PostId = postId }, transaction);
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

    public async Task DeleteAllCommentsByPostIdAsync(int postId, IDbConnection connection, IDbTransaction transaction)
    {
        const string sql = @"
            DELETE FROM Comments
            WHERE PostId = @PostId;
        ";

        await connection.ExecuteAsync(sql, new { PostId = postId }, transaction);
    }

    public async Task<LastPostDTO?> GetLastPostById(int postId)
    {
        var sql = @"
       SELECT 
            p.Id, p.Body, p.ClosedDate, p.CommentCount, p.CreationDate, 
            p.LastEditDate, p.PostTypeId, p.Score, p.Tags, p.Title, p.ViewCount,
            p.OwnerUserId,
            
            c.Id AS CommentId, c.Text, c.CreationDate AS CommentCreationDate, 
            c.PostId, c.Score AS CommentScore, c.UserId, u.DisplayName AS UserDisplayName

        FROM Posts p
        LEFT JOIN (
            SELECT *
            FROM (
                SELECT *, ROW_NUMBER() OVER (PARTITION BY PostId ORDER BY CreationDate DESC) AS rn
                FROM Comments
            ) ranked
            WHERE rn <= 5
        ) c ON c.PostId = p.Id
        LEFT JOIN Users u ON u.Id = c.UserId

        WHERE p.Id = @postId
        ORDER BY c.CreationDate DESC;
    ";


        using var connection = _dapperContext.CreateConnection();
        var postDictionary = new Dictionary<int, LastPostDTO>();

        var result = await connection.QueryAsync<LastPostDTO, CommentDTO, LastPostDTO>(
            sql,
            (post, comment) =>
            {
                if (!postDictionary.TryGetValue(post.Id, out var currentPost))
                {
                    currentPost = post;
                    currentPost.Comments = new List<CommentDTO>();
                    postDictionary.Add(post.Id, currentPost);
                }

                if (comment != null && comment.CommentId != 0)
                {
                    currentPost.Comments.Add(comment);
                }

                return currentPost;
            },
            new { postId },
            splitOn: "CommentId"
        );

        return result.FirstOrDefault();
    }

}
