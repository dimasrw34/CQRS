using System;
using Ardalis.Specification;
using InTouch.UserService.Domain;

namespace InTouch.UserService.Infrastructure.Data;

public class UserByIdAndNameSpecification : Specification<User>
{
    private readonly Guid _id;
    private readonly string _name; 

    public UserByIdAndNameSpecification(Guid id, string name)
    {
        _id = id;
        _name = name;

        Query.Where(user => user.Id == _id && user.Name.Contains(name));
    }
}
