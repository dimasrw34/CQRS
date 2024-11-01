using System;

namespace InTouch.UserService.Query;

/// <summary>
///  Интерфейс query model
/// </summary>
public interface IQueryModel;

/// <summary>
/// Представляет интерфейс для модели запроса с универсальным типом ключа.
/// </summary>
/// <typeparam name="TKey">The type of the key.</typeparam>
public interface IQueryModel<out TKey> : IQueryModel where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Получает идентификатор модели запроса.
    /// </summary>
    TKey Id { get; }
}