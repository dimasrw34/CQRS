using System;
using Dapper;
using InTouch.UserService.Domain;

namespace InTouch.UserService.Application;

public class FindByLoginSpecification (string userName)
 : Specification<User, Guid>(userName)
{
    public (string, DynamicParameters) ToSqlQueryString()=>
        base.ToSqlQueryString();
}
