using Ardalis.Result;

namespace InTouch.UserService.Domain;

public static class PermissionFactory
{
    public static Permission Create(string title) => new(title);
}