using System;
using System.Data;
using System.Threading.Tasks;

namespace InTouch.Infrastructure.Data;

public sealed class DbContext(IDbConnectionFactory connectionFactory) : IDbContext, IDisposable
{
    private readonly IDbConnectionFactory _connectionFactory= connectionFactory;
    private IDbConnection _connection;
    private IDbTransaction _transaction; 
    private IUnitOfWork _unitOfWork;
    public IDbContextState State { get; private set; } = IDbContextState.Closed;

    public IDbConnection Connection =>
        _connection ??= OpenConnection();

    private IDbConnection OpenConnection()
    {
        State = IDbContextState.Open;
        return _connectionFactory.CreateOpenConnection();
    }

    public IDbTransaction Transaction =>
        _transaction ??= Connection.BeginTransaction();

    public IUnitOfWork UnitOfWork =>
        _unitOfWork ??= new UnitOfWork(Transaction);
    
    public async Task CommitAsync()
    {
        try
        {
            await UnitOfWork.CommitAsync();
            State = IDbContextState.Committed;
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
        finally
        {
            Reset();
        }
    }

    public async Task RollbackAsync()
    {
        try
        {
            await UnitOfWork.RollbackAsync();
            State = IDbContextState.RolledBack;
        }
        finally
        {
            Reset();
        }
    }

    public void Dispose()
    {
       //await CommitAsync();
       Reset();
    }
    
    private void Reset()
    {
        Connection?.Close();
        Connection?.Dispose();
        Transaction?.Dispose();

        _connection = null;
        _transaction = null;
        _unitOfWork = null;
    }
}