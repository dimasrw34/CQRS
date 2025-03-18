using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InTouch.UserService.Core;

/// <summary>
/// General repository
/// </summary>
/// <typeparam name="TEntity"></typeparam>
/// <typeparam name="TKey"></typeparam>
public interface IWriteOnlyRepository<TEntity, in TKey> 
    where TEntity : IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TEntity> GetByIdAsync(TKey id, CancellationToken cancellationToken = default);
    Task<Guid> CreateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(TKey id, CancellationToken cancellationToken = default);
}