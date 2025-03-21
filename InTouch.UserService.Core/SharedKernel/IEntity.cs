using System;

namespace InTouch.UserService.Core;

/// <summary>
/// Это базовый интерфейс для всех сущностей.
/// </summary>
public interface IEntity;

/// <summary>
/// Это базовый интерфейс для всех сущностей с ключом.
/// </summary>
/// <typeparam name="TKey">Тип ключа сущности.</typeparam>
public interface IEntity<TKey> : IEntity where TKey : IEquatable<TKey>
{
    Guid Id { get; }
}
