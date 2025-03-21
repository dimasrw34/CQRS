namespace InTouch.UserService.Infrastructure.Authentification;

internal interface IPasswordHasher
{
    string Generate(string password);
    bool Verify(string password, string hashedPassword);
}
