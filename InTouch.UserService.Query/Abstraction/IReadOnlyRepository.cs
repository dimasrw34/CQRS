using System;
using System.Threading.Tasks;

namespace InTouch.UserService.Query;

/// <summary>
/// Интерфейс репозитория только для чтения
/// </summary>
/// <typeparam name="TQueryModel">Тип модели запроса.</typeparam>
/// <typeparam name="TKey">Тип ключа для модели запроса.</typeparam>
public interface IReadOnlyRepository<TQueryModel, in TKey>
    where TQueryModel : IQueryModel<TKey>
    where TKey : IEquatable<TKey>
{
    /// <summary>
    /// Асинхронно получает модель запроса по ее идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Задача, представляющая асинхронную операцию, возвращающую модель запроса.</returns>
    Task<TQueryModel> GetByIdAsync(TKey id);
}