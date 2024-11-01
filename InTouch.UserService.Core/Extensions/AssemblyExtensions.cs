using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InTouch.UserService.Core;

public static class AssemblyExtensions
{
    /// <summary>
    /// Извлекает все типы в указанной сборки, которые реализуют заданный интерфейс или базовый класс
    /// или наследуют его. Используется в NoSqlDbContext
    /// </summary>
    /// <param name="assembly">Тип интерфейса или базового класса.</param>
    /// <typeparam name="TInterface">Сборка для поиска.</typeparam>
    /// <returns>Перечисляемая коллекция типов, которые реализуют указанный интерфейс или базовый класс
    /// или наследуют его.</returns>
    public static IEnumerable<Type> GetAllTypesOf<TInterface>(this Assembly assembly)
    {
        var isAssignableToIInterface = typeof(TInterface).IsAssignableFrom;
        return assembly
            .GetTypes()
            .Where(type => type.IsClass && !type.IsAbstract &&
                           !type.IsInterface && isAssignableToIInterface(type))
            .ToList();
    }
}