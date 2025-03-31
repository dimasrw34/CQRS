using System;
using System.Threading;
using System.Threading.Tasks;

using InTouch.UserService.Core;

namespace InTouch.UserService.Infrastructure.Data;

/// <summary>
/// General repository
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IWriteOnlyRepository<TEntity, TKey> : IEventStoreRepository
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey> 
{
    Task<TKey> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);
}