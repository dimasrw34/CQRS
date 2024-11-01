using System.Threading.Tasks;
using MongoDB.Driver;

namespace InTouch.UserService.Query;

/// <summary>
/// Представляет контекст базы данных, доступный только для чтения, для запроса данных.
/// </summary>
public interface IReadDbContext
{
    /// <summary>
    /// Получает строку подключения к базе данных.
    /// </summary>
    string ConnectionString { get; }

    /// <summary>
    /// Получает коллекцию для указанной модели запроса.
    /// </summary>
    /// <typeparam name="TQueryModel">Тип модели запроса. </typeparam>
    /// <returns>Коллекция MongoDB для указанной модели запроса.</returns>
    IMongoCollection<TQueryModel> GetCollection<TQueryModel>() where TQueryModel : IQueryModel;

    /// <summary>
    /// Создает коллекции в базе данных для всех моделей запросов.
    /// </summary>
    /// <returns>Таска, представляющая собой асинхронное создание коллекций.</returns>
    Task CreateCollectionAsync();
}