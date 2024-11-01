using System;

namespace InTouch.UserService.Query;

public class UserQueryModel : IQueryModel<Guid>
{
    public UserQueryModel(
        Guid id, 
        string email, 
        string password, 
        string name, 
        string surname, 
        string phone)
    {
        Id = id;
        Email = email;
        Password = password;
        Name = name;
        Surname = surname;
        Phone = phone;
    }

    public Guid Id { get; private init; }
    public string Email { get; private init;  }
    public string Password { get; private init;  }
    public string Name { get; private init;  }
    public string Surname { get; private init;  }
    public string Phone { get; private init;  }
}