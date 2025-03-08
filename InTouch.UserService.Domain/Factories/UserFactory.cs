using System.Linq;
using Ardalis.Result;

namespace InTouch.UserService.Domain;

public static class UserFactory
{
    /// <summary>
    /// Используется для тестирования
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="phone"></param>
    /// <returns></returns>
    public static Result<User> Create(
        string email, 
        string password, 
        string firstName, 
        string lastName,
        string phone)
    {
        var emailResult = Email.Create(email);
        return !emailResult.IsSuccess
            ? Result<User>.Error(new ErrorList(emailResult.Errors.ToArray()))
            : Result<User>.Success(new User(emailResult.Value, password, firstName,lastName, phone));
    }

    /// <summary>
    /// Фабричный метод по созданию объекта
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="firstName"></param>
    /// <param name="lastName"></param>
    /// <param name="phone"></param>
    /// <returns></returns>
    public static User Create(Email email, string password, string firstName, string lastName, string phone) 
        => new(email, password, firstName,lastName, phone);
    
}