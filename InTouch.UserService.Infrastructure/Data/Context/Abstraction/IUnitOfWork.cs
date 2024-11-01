using System.Data;
using System.Threading.Tasks;

namespace InTouch.Infrastructure.Data;
public interface IUnitOfWork
{
    IUnitOfWorkState Sate { get; }
    IDbTransaction Transaction { get; }
    Task CommitAsync();
    Task RollbackAsync();
}

