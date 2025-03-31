using System;
using Ardalis.Specification;
using InTouch.UserService.Domain;

namespace InTouch.UserService.Infrastructure.Data;
public class UserByIdSpecification : Specification<User>
{
    private readonly Guid _id;
    public UserByIdSpecification(Guid id)
    {
        _id = id;
        Query.Where(user => user.Id == _id);
    }
}