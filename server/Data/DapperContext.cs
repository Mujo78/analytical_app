using System;
using System.Data;
using Microsoft.Data.SqlClient;
using Dapper;
using StackExchange.Profiling;

namespace server.Data;

public class DapperContext
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;

    public DapperContext(IConfiguration configuration)
    {
        _configuration = configuration;
        _connectionString = _configuration.GetConnectionString("DefaultConnection")!;
    }

    public IDbConnection CreateConnection()
    {
        IDbConnection connection = new SqlConnection(_connectionString);
        return new StackExchange.Profiling.Data.ProfiledDbConnection((System.Data.Common.DbConnection)connection, MiniProfiler.Current);
    }
}
