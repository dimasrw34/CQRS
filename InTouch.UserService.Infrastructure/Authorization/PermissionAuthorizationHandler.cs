using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InTouch.UserService.Infrastructure.Authentification;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace InTouch.UserService.Infrastructure.Authorization;

public class PermissionAuthorizationHandler: AuthorizationHandler<PermissionRequirement>
{
private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILogger _logger;
    
    public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory, ILogger<PermissionAuthorizationHandler> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _logger = logger;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        try
        {
            var userId = context.User.Claims.FirstOrDefault(
                c => c.Type == CustomClaims.UserId);

            Guid.TryParse(userId?.Value, out var id);

            using var scope = _serviceScopeFactory.CreateScope();
            var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionService>();

            var permissions = await permissionService.GetPermissionsAsync(id);

            HashSet<Permission> requirementsSet = new HashSet<Permission>(requirement.Permissions);

            if (requirementsSet.IsSubsetOf(permissions))
            {
                context.Succeed(requirement);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка в HandleRequirementAsync");
            throw;
        }
    }
}
