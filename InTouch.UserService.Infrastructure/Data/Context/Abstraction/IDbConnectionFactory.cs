using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace InTouch.UserService.Infrastructure.Data;

public interface IDbConnectionFactory
{
    Task<NpgsqlConnection> CreateOpenConnectionAsync(CancellationToken cancellationToken);
    NpgsqlConnection GetConnection { get; }
}