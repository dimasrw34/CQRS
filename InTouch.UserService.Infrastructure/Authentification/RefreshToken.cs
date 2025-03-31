using System;
using InTouch.UserService.Domain;

namespace InTouch.UserService.Infrastructure.Authentification;

internal sealed class RefreshToken
{
    public Guid Id { get; set; }
    public string? Token { get; set; }
    public Guid UserID { get; set; }
    public DateTime ExpiresOnUtc { get; set; }
    public User User { get; set; }
}
