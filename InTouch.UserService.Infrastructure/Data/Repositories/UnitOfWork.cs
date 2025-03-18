using System;
using System.Collections.Concurrent;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InTouch.UserService.Core;
using Npgsql;

namespace  InTouch.Infrastructure.Data;

public class UnitOfWork: IUnitOfWork 
{

    private readonly IDbConnection _connection;
    private readonly IDbTransaction? _transaction;
    private readonly ConcurrentDictionary<Type, object> _repositories;

    public UnitOfWork(IDbConnectionFactory connectionFactory, CancellationToken cancellationToken=default)
    {
        using var connectionTask = connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        _connection = connectionTask.Result;
        _transaction = _connection.BeginTransaction();
        _repositories = new ConcurrentDictionary<Type, object>();
    }

   public IWriteOnlyRepository<TEntity, TKey> GetRepository<TEntity, TKey>() 
        where TKey : IEquatable<TKey> 
        where TEntity : IEntity<TKey>

    {
        /*var repositoryType = typeof(BaseWriteOnlyRepository<TEntity, TKey>).MakeGenericType(typeof(TEntity), typeof(TKey));
        
        return (IWriteOnlyRepository<TEntity,TKey>)_repositories.GetOrAdd(typeof(IWriteOnlyRepository<TEntity,TKey>), 
            _ => Activator.CreateInstance(repositoryType, _connection, this)!);*/
        // Получаем определение типа репозитория с generic параметрами
        Type repositoryDefinition = typeof(BaseWriteOnlyRepository<,>);
    
        // Создаём конкретный тип
        Type concreteType = repositoryDefinition.MakeGenericType(typeof(TEntity), typeof(TKey));
    
        return (IWriteOnlyRepository<TEntity, TKey>)_repositories.GetOrAdd(
            typeof(IWriteOnlyRepository<TEntity, TKey>),
            _ => Activator.CreateInstance(concreteType, _connection, this)!);
    }
    
    public IDbTransaction Transaction => _transaction ?? throw new InvalidOperationException("Транзакция не начата");
    
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (cancellationToken.IsCancellationRequested)
            {
                throw new OperationCanceledException();
            }

            Console.WriteLine("sd");
            await Task.Run(() => _transaction!.Commit(), cancellationToken);
        }
        catch (OperationCanceledException)
        {
            await Task.Run(() => _transaction!.Rollback(), cancellationToken);
            throw;
        }
        catch
        {
            await Task.Run(() => _transaction!.Rollback(), cancellationToken);
            throw;
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_transaction != null)
        {
            await Task.Run(() => _transaction.Dispose());
        }
        await Task.Run(() => _connection.Dispose());
    }

    public void Dispose()
    {
        DisposeAsync().GetAwaiter().GetResult();
    }
}

