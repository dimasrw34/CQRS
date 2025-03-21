using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InTouch.UserService.Core;
using InTouch.UserService.Domain;
using Npgsql;

namespace  InTouch.Infrastructure.Data;

public sealed class UnitOfWork : IUnitOfWork
{

    private readonly IDbConnection _connection;
    private readonly IDbTransaction? _transaction;

    private readonly IDbConnectionFactory _connectionFactory;
    private readonly Lazy<ConcurrentDictionary<Type, object>> _cache;

    public UnitOfWork(IDbConnectionFactory connectionFactory, CancellationToken cancellationToken = default)
    {
        _connectionFactory = connectionFactory;
        using var connectionTask = connectionFactory.CreateOpenConnectionAsync(cancellationToken);
        _connection = connectionTask.Result;
        _transaction = _connection.BeginTransaction();
        //Создаем словарь для кэширования
        _cache = new Lazy<ConcurrentDictionary<Type, object>>(() => 
            new ConcurrentDictionary<Type, object>());
   }

    public IWriteOnlyRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
        where TKey : IEquatable<TKey>
        where TEntity : IEntity<TKey>

    {
        // Создаем ключ для кэша на основе типов TEntity и TKey
        var cacheKey = typeof(TEntity);

        // Пытаемся получить репозиторий из кэша
        if (_cache.Value.TryGetValue(cacheKey, out object repository))
        {
            return (IWriteOnlyRepository<TEntity, TKey>)repository;
        }

        // Если репозиторий не найден в кэше, создаем новый
        var newRepository = new BaseWriteOnlyRepository<TEntity, TKey>(
            _connectionFactory,
            this
        );

        // Сохраняем репозиторий в кэш
        _cache.Value.TryAdd(cacheKey, newRepository);

        return newRepository;
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

    public async Task RollbackChangesAsync(CancellationToken cancellationToken = default)
    {
        await Task.Run(() => _transaction!.Rollback(), cancellationToken);
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

