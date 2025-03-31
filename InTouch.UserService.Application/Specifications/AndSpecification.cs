using System;
using System.Text;
using Dapper;
using InTouch.UserService.Core;

namespace InTouch.UserService.Application;

public class AndSpecification<TEntity, TKey>(
    Specification<TEntity, TKey> left,
    Specification<TEntity, TKey> right)
    : ISpecification<TEntity, TKey>
    where TEntity : class, IEntity<TKey> 
    where TKey : IEquatable<TKey>
{
    private readonly Specification<TEntity, TKey> _leftSpecification = left;
    private readonly Specification<TEntity, TKey> _rightSpecification = right;

    public (string, DynamicParameters) ToSqlQueryString()
    {
        //формируем 2 запроса
        var leftString = _leftSpecification.ToSqlQueryString();
        var rightString = _rightSpecification .ToSqlQueryString();
        
        //объединяем в 1 запрос
        var Query = CombineSqlQueries(leftString.Item1, rightString.Item1);

        var builder = new StringBuilder();
        
        return (Query, leftString.Item2);
    }

    private string CombineSqlQueries (string firstQuery, string secondQuery)
    {
        var builder = new StringBuilder();
    
        // Добавляем базовую часть первого запроса
        builder.Append(firstQuery.Split(new[] { "WHERE" }, 
        StringSplitOptions.None)[0].Trim());
    
        // Добавляем WHERE и первое условие
        builder.Append(" WHERE ");
        builder.Append(firstQuery.Split(new[] { "WHERE" }, 
        StringSplitOptions.None)[1].Trim());
    
        // Добавляем AND и второе условие
        builder.Append(" AND ");
        builder.Append(firstQuery.Split(new[] { "WHERE" }, 
        StringSplitOptions.None)[1].Trim());
    
        return builder.ToString();
    }
}