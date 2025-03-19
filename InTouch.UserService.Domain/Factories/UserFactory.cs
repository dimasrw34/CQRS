using System;
using System.Linq;
using Ardalis.Result;

namespace InTouch.UserService.Domain;

public static class UserFactory
{
    /// <summary>
    /// Используется для тестирования
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="phone"></param>
    /// <returns></returns>
    public static Result<User> Create(
        string login,
        string password, 
        string firstName, 
        string lastName,
        string email, 
        string phone)
    {
        var emailResult = Email.Create(email);
        return !emailResult.IsSuccess
            ? Result<User>.Error(new ErrorList(emailResult.Errors.ToArray()))
            : Result<User>.Success(new User(login, password, firstName,lastName, emailResult.Value, phone));
    }

    /// <summary>
    /// Фабричный метод по созданию объекта
    /// </summary>
    /// <param name="login"></param>
    /// <param name="password"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="email"></param>
    /// <param name="phone"></param>
    /// <returns></returns>
    public static User Create(string login,  string password, string firstName, string lastName, Email email, string phone) 
        => new(login, password, firstName,lastName,email, phone);
    
}