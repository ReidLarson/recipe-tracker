using System.Data;

namespace RecipeTracker.Core.Data;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync();
}