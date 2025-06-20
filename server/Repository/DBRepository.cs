using System;
using System.Data;
using server.Data;
using server.Repository.IRepository;
using Dapper;

namespace server.Repository;

public class DBRepository(DapperContext _dapperContext) : IDBRepository
{
    private readonly DapperContext _dapperContext = _dapperContext;

    public async Task CreateIndexes()
    {
        var sql = @"
            IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Posts_OwnerUserId')
            CREATE NONCLUSTERED INDEX IX_Posts_OwnerUserId ON Posts (OwnerUserId);

            IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Posts_CreationDate')
            CREATE NONCLUSTERED INDEX IX_Posts_CreationDate ON Posts (CreationDate);

            IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_Reputation_CreationDate')
            CREATE NONCLUSTERED INDEX IX_Users_Reputation_CreationDate ON [dbo].[Users] ([Reputation])
            INCLUDE ([CreationDate],[DisplayName],[DownVotes],[LastAccessDate],[UpVotes],[Views])

            IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Comments_PostId')
            CREATE NONCLUSTERED INDEX IX_Comments_PostId ON Comments (PostId);

            IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Comments_UserId')
            CREATE NONCLUSTERED INDEX IX_Comments_UserId ON Comments (UserId);
        ";

        var connection = _dapperContext.CreateConnection();
        connection.Open();

        await connection.ExecuteAsync(sql);
    }

    public async Task DropIndexes()
    {
        var sql = @"
            IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Posts_OwnerUserId')
            DROP INDEX IX_Posts_OwnerUserId ON Posts;

            IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Posts_CreationDate')
            DROP INDEX IX_Posts_CreationDate ON Posts;

            IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Users_Reputation_CreationDate')
            DROP INDEX IX_Users_Reputation_CreationDate ON Users;

            IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Comments_PostId')
            DROP INDEX IX_Comments_PostId ON Comments;

            IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Comments_UserId')
            DROP INDEX IX_Comments_UserId ON Comments;
        ";

        var connection = _dapperContext.CreateConnection();
        connection.Open();

        await connection.ExecuteAsync(sql);
    }
}
