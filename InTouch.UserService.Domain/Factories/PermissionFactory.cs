
namespace InTouch.UserService.Domain;

public static class PermissionFactory
{
    public static PermissionIT Create(string title) => new(title);
}