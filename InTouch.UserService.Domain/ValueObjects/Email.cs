using Ardalis.Result;
using InTouch.UserService.Core;

namespace InTouch.UserService.Domain;

//Ardalis.Result - абстракция результата, которую при необходимости можно сопоставить с кодами ответов HTTP.
//HowTo - https://result.ardalis.com/

/// <summary>
/// Почтовый адрес 
/// </summary>
public sealed record Email : IValueObject
{
    /// <summary>
    /// Конструктор по умолчанию для ORM
    /// </summary>
    public Email(){ }

    /// <summary>
    /// Создание нового инстанса <see cref="Email"/> класса.
    /// </summary>
    /// <param name="address">Адрес почты</param>
    public Email(string address) =>
        Address = address;

    /// <summary>
    /// Получение адреса почты
    /// </summary>
    public string Address { get; }

    /// <summary>
    ///  Создает новый <see cref="Email"/> инстанс.
    /// </summary>
    /// <param name="emailAddress"></param>
    /// <returns></returns>
    public static Result<Email> Create(string emailAddress)
    {
        if (string.IsNullOrWhiteSpace(emailAddress))
        {
            return Result<Email>.Error("Необходимо указать адрес электронной почты.");
        }

        return !RegexPatterns.EmailsValid.IsMatch(emailAddress)
            ? Result<Email>.Error("Адрес электронной почты недействителен.")
            : Result<Email>.Success(new Email(emailAddress));
    }

    /// <summary>
    /// Возвращает строку, представляющую текущий <see cref="Email"/> объект.
    /// </summary>
    /// <returns>Возвращает строку, представляющую текущий <see cref="Email"/> объект.</returns>
    public override string ToString() => Address;
}