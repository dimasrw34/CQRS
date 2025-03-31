using System;
using Dapper;
using InTouch.UserService.Core;



namespace InTouch.UserService.Core;

public interface ISpecification <TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : IEquatable<TKey>
{
    (string, DynamicParameters) ToSqlQueryString();
}
