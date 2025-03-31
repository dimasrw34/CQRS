using System;
using InTouch.UserService.Core;

namespace InTouch.UserService.Domain;

public sealed class UserUpdatedEvent (
    Guid id,
    string login,
    string password,
    string name,
    string lastname,
    string email,
    string phone)
    : UserBaseEvent (id, login, password, name, lastname, email, phone);