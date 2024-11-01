using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTouch.UserService.Core;

public interface ICacheService
{
    /// <summary>
    /// Получает элемент (item) из кэша, если он существует, в противном случае создает элемент с помощью
    /// предоставленной фабричной функции и добавляет его в кэш.
    /// </summary>
    /// <param name="cacheKey">Ключ для идентификации элемента в кэше.</param>
    /// <param name="factory">Фабричная функция для создания элемента, если его нет в кэше.</param>
    /// <typeparam name="TItem">Тип элемента, который нужно получить или создать.</typeparam>
    /// <returns>Элемент из кэша или вновь созданный элемент.</returns>
    Task<TItem> GetOrCreateAsync<TItem>(string cacheKey, Func<Task<TItem>> factory);
    
    /// <summary>
    /// Получает список элементов из кэша, если он существует, в противном случае создает элементы с помощью
    /// предоставленной фабричной функции и добавляет их в кэш.
    /// </summary>
    /// <param name="cacheKey">>Ключ для идентификации элемента в кэше.</param>
    /// <param name="factory">Фабричная функция для создания элементов, если их нет в кэше.</param>
    /// <typeparam name="TItem">Тип элемента, который нужно получить или создать.</typeparam>
    /// <returns></returns>
    Task<IReadOnlyList<TItem>> GetOrCreateAsync<TItem>(string cacheKey, Func<Task<IReadOnlyList<TItem>>> factory);
    
    /// <summary>
    /// Удаляет элементы с указанными ключами кэша из кэша.
    /// </summary>
    /// <param name="cacheKeys">Ключи элементов, которые необходимо удалить из кэша.</param>
    /// <returns>Задача, представляющая собой асинхронную операцию удаления.</returns>
    Task RemoveAsync(params string[] cacheKeys);
}