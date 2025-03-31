using System;
using Dapper;
using InTouch.UserService.Domain;


namespace InTouch.UserService.Application;

public class FindByIdSpecification (Guid parameter) 
    : Specification<User, Guid>(parameter)
{
    public (string, DynamicParameters) ToSqlQueryString() =>
        base.ToSqlQueryString();

}
