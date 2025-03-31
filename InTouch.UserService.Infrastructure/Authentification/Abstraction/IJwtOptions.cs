namespace InTouch.UserService.Infrastructure.Authentification;

interface IJwtOptions
{
    string SecretKey { get; set; }
    int ExpiresHours { get; set; }
}
