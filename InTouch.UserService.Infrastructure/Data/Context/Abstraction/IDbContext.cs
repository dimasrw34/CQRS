using System.Data;
using System.Threading.Tasks;

namespace InTouch.Infrastructure.Data;

public interface IDbContext
{
    IDbContextState State { get; }
    IDbConnection Connection { get; }
    IDbTransaction Transaction { get; }
    
    IUnitOfWork UnitOfWork { get; }
    Task CommitAsync();
    Task RollbackAsync();
}