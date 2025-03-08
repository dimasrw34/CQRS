using System;
using System.Data;
using System.Threading.Tasks;
using System.Transactions;

namespace InTouch.Infrastructure.Data;

public sealed class UnitOfWork(IDbTransaction transaction) : IUnitOfWork
{
    public IUnitOfWorkState Sate { get; private set; } = IUnitOfWorkState.Open;
    public IDbTransaction Transaction { get; private set; } = transaction;
    
    public async Task CommitAsync()
    {
        try
        {
            await Task.Run(() =>
            {
                
                Transaction.Commit();
            });
            Sate = IUnitOfWorkState.Committed;
        }
        catch (Exception e)
        {
            Transaction.Rollback();
            throw;
        }
    }

    public async Task RollbackAsync()
    {
        await Task.Run(() =>
            {
                Transaction.Rollback();
            }
        );
        Sate = IUnitOfWorkState.RolledBack;
    }
}