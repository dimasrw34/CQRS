using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace InTouch.Infrastructure;

// Помощник для работы со свойствами
public static class PropertyHelper
{
    public static Dictionary<string, PropertyInfo> GetColumnMappings(Type entityType)
    {
        return entityType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.CanWrite)
            .ToDictionary(
                p => p.Name.ToLower(),
                p => p
            );
    }

    public static string[] GetColumnNames(Type entityType)
    {
        return GetColumnMappings(entityType)
            .Select(kvp => $"\"{kvp.Key}\"")
            .ToArray();
    }
}