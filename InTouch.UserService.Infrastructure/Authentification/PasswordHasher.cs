using System;

namespace InTouch.UserService.Infrastructure.Authentification;

internal sealed class PasswordHasher : IPasswordHasher
{
    //генерируем hash пароль
    public string Generate(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password);

    //подстверждаем, что введеный пароль соответсвует хэшированному
    // когда логинем пользователя
    public bool Verify(string password, string hashedPassword) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, hashedPassword);

}
