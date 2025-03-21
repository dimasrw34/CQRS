using System;

namespace InTouch.UserService.Infrastructure.Authentification;

public sealed class JwtOptions : IJwtOptions
{
    //секретный ключ
    public string SecretKey { get; set; } = string.Empty;
    //сколько часов будет действовать токен
    public int ExpiresHours { get; set; } = default;
}
