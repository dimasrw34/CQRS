using System;
using System.Threading;
using System.Threading.Tasks;

namespace InTouch.UserService.Core;

/// <summary>
/// Представляет собой единицу работы по управлению операциями базы данных.
/// </summary>
public interface IUnitOfWork<TEntity,TKey> : IDisposable 
    where TEntity : IEntity <TKey>
    where TKey : IEquatable<TKey>
    
{
    /// <summary>
    /// Сохраняет изменения, внесенные в единицу работы, асинхронно.
    /// </summary>
    /// <returns>Задача, представляющая асинхронную операцию.</returns>
    Task SaveChanges(TEntity entity, EventStore eventStore,CancellationToken cancellationToken);
}