using System;
using server.Data;
using server.Models;
using server.Repository.IRepository;
using Dapper;
using server.DTO.Comment;
using server.Repository.IRepository.IComment;

namespace server.Repository;

public class CommentDapperRepository(DapperContext dapperContext) : ICommentDapperRepository
{
    private readonly DapperContext _dapperContext = dapperContext;
    /// <summary>
    /// Dapper implementation of ICommentRepository
    /// </summary>
    public async Task DeleteCommentAsync(Comment comment)
    {
        var sqlUpdatePost = @"
            UPDATE Posts
            SET CommentCount = CommentCount - 1
            WHERE Id = @PostId;
        ";

        var sqlDeleteComment = @"
            DELETE FROM Comments
            WHERE Id = @CommentId;
        ";

        using var connection = _dapperContext.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        try
        {
            await connection.ExecuteAsync(sqlUpdatePost, new { PostId = comment.PostId }, transaction);
            await connection.ExecuteAsync(sqlDeleteComment, new { CommentId = comment.Id }, transaction);

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task<Comment?> GetCommentByIdAsync(int commentId)
    {
        var sql = @"
            SELECT 
                c.*, 
                p.*
            FROM Comments c
            INNER JOIN Posts p ON c.PostId = p.Id
            WHERE c.Id = @CommentId;
        ";

        using var connection = _dapperContext.CreateConnection();

        var result = await connection.QueryAsync<Comment, Post, Comment>(
            sql,
            (comment, post) =>
            {
                comment.Post = post;
                return comment;
            },
            new { CommentId = commentId }
        );

        return result.FirstOrDefault();
    }

    public async Task<List<CommentDTO>> GetCommentsByPostIdAsync(int postId)
    {
        var sql = @"
            SELECT TOP 10 
                c.Id,
                c.Text,
                c.CreationDate,
                ISNULL(c.UserId, 0) AS UserId,
                ISNULL(u.DisplayName, '') AS UserDisplayName,
                ISNULL(c.Score, 0) AS Score,
                c.PostId
            FROM Comments c
            LEFT JOIN Users u ON c.UserId = u.Id
            WHERE c.PostId = @PostId
            ORDER BY c.CreationDate DESC;
        ";

        using var connection = _dapperContext.CreateConnection();

        var comments = await connection.QueryAsync<CommentDTO>(sql, new { PostId = postId });

        return [.. comments];
    }
}
