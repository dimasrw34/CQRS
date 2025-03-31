using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InTouch.UserService.Core;
using InTouch.UserService.Domain;

namespace InTouch.UserService.Infrastructure.Data;

/// <summary>
/// Представляет собой единицу работы по управлению операциями базы данных.
/// </summary>
public interface IUnitOfWork : IDisposable 
{
    /// <summary>
    /// Сохраняет изменения, внесенные в единицу работы, асинхронно.
    /// </summary>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken);

    Task RollbackChangesAsync(CancellationToken cancellationToken);
    public UserWriteOnlyRepository<User,Guid> Users {get;}
/*
    IWriteOnlyRepository<TEntity, TKey> GetRepository<TEntity, TKey>() 
        where TEntity : class, IEntity<TKey> 
        where TKey : IEquatable<TKey>;
  */  
    IDbTransaction Transaction { get; }

}