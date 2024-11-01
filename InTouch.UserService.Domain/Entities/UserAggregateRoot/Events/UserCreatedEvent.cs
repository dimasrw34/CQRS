using System;

namespace InTouch.UserService.Domain;

public sealed class UserCreatedEvent (
    Guid id,
    string email,
    string password,
    string name,
    string surname,
    string phone)
    : UserBaseEvent(id, email, password, name, surname, phone);