using System.Data;

namespace InTouch.Infrastructure.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateOpenConnection();
}