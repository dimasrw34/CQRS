using System;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace InTouch.UserService.Query.Data.Repositories;

/// <summary>
/// Базовый класс репозитория для операций только для чтения.
/// </summary>
/// <param name="context">Контекст чтения базы данных.</param>
/// <typeparam name="TQueryModel">Тип модели запроса.</typeparam>
/// <typeparam name="TKey">Тип ключа.</typeparam>
/// <remarks>
/// Инициализирует новый экземпляр <see cref="BaseReadOnlyRepository{TQueryModel, Tkey}"/> класса.
/// </remarks>
public abstract class BaseReadOnlyRepository<TQueryModel, TKey> (IReadDbContext context) : IReadOnlyRepository<TQueryModel, TKey>
    where TQueryModel : IQueryModel<TKey>
    where TKey : IEquatable<TKey>
{
    protected readonly IMongoCollection<TQueryModel> Collection = context.GetCollection<TQueryModel>();
    
    /// <summary>
    /// Получает модель запроса по ее идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор модели запроса.</param>
    /// <returns>Модель запроса.</returns>
    public async Task<TQueryModel> GetByIdAsync(TKey id)
    {
        using var asyncCursor = await Collection.FindAsync(queryModel => queryModel.Id.Equals(id));
        return await asyncCursor.SingleOrDefaultAsync();
    }
}