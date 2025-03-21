namespace InTouch.UserService.Infrastructure.Authentification;

internal sealed record TokensResponse(string AccessToken, string RefreshToken);
