using System.Data;

namespace Agronomia.Infrastructure.Dapper;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}
