using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace InTouch.Infrastructure.Data;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken);
}