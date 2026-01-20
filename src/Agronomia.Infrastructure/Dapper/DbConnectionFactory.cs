using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace Agronomia.Infrastructure.Dapper;

public sealed class DbConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    private readonly string _connectionString = configuration.GetConnectionString("WriteDatabase")
        ?? throw new InvalidOperationException("ConnectionStrings:WriteDatabase is not configured.");

    public IDbConnection CreateConnection()
    {
        var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
        return connection;
    }
}
