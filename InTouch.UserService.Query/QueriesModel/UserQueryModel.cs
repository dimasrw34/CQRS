using System;
using InTouch.UserService.Core;

namespace InTouch.UserService.Query;

public class UserQueryModel : IQueryModel<Guid>
{
    public UserQueryModel(
        Guid id, 
        string login, 
        string password, 
        string name, 
        string lastname,
        string email,
        string phone)
    {
        Id = id;
        Login = login;
        Password = password;
        Name = name;
        LastName = lastname;
        Email = email;
        Phone = phone;
    }

    public Guid Id { get; private init; }
    public string Login { get; private init;  }
    public string Password { get; private init;  }
    public string Name { get; private init;  }
    public string LastName { get; private init;  }
    public string Email { get; private init;  }
    public string Phone { get; private init;  }
}