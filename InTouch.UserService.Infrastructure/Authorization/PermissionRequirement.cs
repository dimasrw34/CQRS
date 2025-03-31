using Microsoft.AspNetCore.Authorization;

namespace InTouch.UserService.Infrastructure.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string policyName;
    public PermissionRequirement(Permission[] permissions)
    {
        Permissions = permissions;
    }
   public Permission[] Permissions { get; set; } = [];
}

