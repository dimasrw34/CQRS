using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InTouch.UserService.Infrastructure.Authorization;

internal interface IPermissionService
{
    Task<HashSet<Permission>> GetPermissionsAsync(Guid userId);
}
