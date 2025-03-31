using Ardalis.Specification;
using InTouch.UserService.Domain;

namespace InTouch.UserService.Infrastructure.Data;


public class UserByLoginSpecification :SingleResultSpecification<User>
{
    private readonly string _login;

    public UserByLoginSpecification(string login)
    {
        _login = login;
        Query.Where(user => user.Name.Contains(_login));
    }
}
