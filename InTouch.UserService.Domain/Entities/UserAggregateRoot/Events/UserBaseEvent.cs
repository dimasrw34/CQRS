using System;
using InTouch.UserService.Core;

namespace InTouch.UserService.Domain;

public abstract class UserBaseEvent : BaseEvent
{
    protected UserBaseEvent(
        Guid id,
        string email,
        string password,
        string name,
        string surname,
        string phone
    )
    {
        Id = id;
        AggregateID = id;
        Email = email;
        Password = password;
        Name = name;
        Surname = surname;
        Phone = phone;
    }

    public Guid Id { get; private init; }
    public string Email { get; private init; }
    public string Password { get; private init; }
    public string Name { get; private init; }
    public string Surname { get; private init; }
    public string Phone { get; private init; }
}