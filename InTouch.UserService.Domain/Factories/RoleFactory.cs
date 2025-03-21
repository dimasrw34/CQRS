namespace InTouch.UserService.Domain;

public static class RoleFactory
{
    public static Role Create(string title) => new(title);
}