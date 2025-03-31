using System;
using Dapper;
using InTouch.UserService.Domain;

namespace InTouch.UserService.Application;

public class FindByIdAndLoginSpecification
(FindByIdSpecification left,
FindByLoginSpecification right) 
    :AndSpecification<User, Guid>(left, right)
{
    public (string, DynamicParameters) ToSqlQueryString() =>
        base.ToSqlQueryString();
}
