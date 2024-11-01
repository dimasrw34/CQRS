using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace InTouch.UserService.Query;

public interface ISynchronizedDb
{
    /// <summary>
    /// Вставляет модель запроса в базу данных.
    /// </summary>
    /// <param name="queryModel">Тип модели запроса.</param>
    /// <param name="upsertFilter">Модель запроса для обновления и вставки.</param>
    /// <typeparam name="TQueryModel">Выражение фильтра для определения условия обновления.</typeparam>
    /// <returns>Таска, представляющая собой асинхронную операцию обновления и вставки.</returns>
    Task UpsertAsync<TQueryModel>(TQueryModel queryModel, Expression<Func<TQueryModel,bool>> upsertFilter)
        where TQueryModel : IQueryModel;

    /// <summary>
    /// Удаляет модели запросов из базы данных, соответствующие указанному фильтру.
    /// </summary>
    /// <param name="deleteFilter">Тип модели запроса.</param>
    /// <typeparam name="TQueryModel">Выражение фильтра для определения того, какие модели запроса следует удалить.</typeparam>
    /// <returns>Таска, представляющая собой асинхронную операцию удаления.</returns>
    Task DeleteAsync<TQueryModel>(Expression<Func<TQueryModel, bool>> deleteFilter)
        where TQueryModel : IQueryModel;
}