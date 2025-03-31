using System;
using System.Linq;
using System.Reflection;
using Dapper;
using InTouch.UserService.Core;



namespace InTouch.UserService.Application;

public class Specification<TEntity, TKey>(object parameter) 
    : ISpecification <TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    private readonly object _parameter = parameter;
    
    public (string, DynamicParameters) ToSqlQueryString()
    {
        //узнаем название класс
        var tableName = typeof(TEntity).Name;

        // Получаем все свойства сущности за исключением события
        var allProperties = typeof(TEntity).GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.Name != "DomainEvents")
            .ToList();
        // Формируем список колонок с учётом кавычек для PostgreSQL
        var columns = string.Join(", ", allProperties.Select(p => $"\"{p.Name}\"")).ToLower();
        // Формируем параметры, добавляя @param0 в начало
        var values = string.Join(", ", allProperties.Select((p, i) => "@param" + i));

        //узнаем имя свойства по которому будем делать отбор и значение
        var paramObj = GetProperties (_parameter);
        
        //формируем строку запроса
        var sql = $"SELECT ({columns}) FROM {tableName}s WHERE @{paramObj.propertyName} = {paramObj.propertyValue.ToString()};";

        //формируем набор параметров для передачи в dapper
        var parameters = new DynamicParameters();
        int paramIndex = 0;

        foreach (var property in allProperties)
        {
            var propertyValue = property.GetValue(typeof(TEntity));

            // Если свойство реализует IValueObject, используем его значение
            if (propertyValue is IValueObject valueObject)
            {
                parameters.Add($"param{paramIndex}", valueObject.ToString());
            }
            else
            {
                parameters.Add($"param{paramIndex}", propertyValue);
            }
            paramIndex++;
        }
        return (sql, parameters);
    }

    private (string propertyName, object propertyValue) GetProperties(object parameter)
    {
        if (parameter == null)
            return (null, null);

        // Если параметр является свойством объекта
        var propertyInfo = parameter.GetType().GetProperties()
            .FirstOrDefault(p => p.GetValue(parameter) == parameter);

        if (propertyInfo != null)
        {
            return (propertyInfo.Name, propertyInfo.GetValue(parameter));
        }

        // Если это примитивный тип
        return (parameter.GetType().Name, parameter);    
    }
}