using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;


namespace InTouch.UserService.Core;

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

    IWriteOnlyRepository<TEntity, TKey> GetRepository<TEntity, TKey>() 
        where TEntity : IEntity<TKey> 
        where TKey : IEquatable<TKey>;
    
    IDbTransaction Transaction { get; }

}