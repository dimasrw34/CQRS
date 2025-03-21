using InTouch.UserService.Domain;

namespace InTouch.UserService.Infrastructure.Authentification;

internal interface IJwtProvider
{
    string GenerateRefreshToken();
    string GenerateToken(User user);
}
