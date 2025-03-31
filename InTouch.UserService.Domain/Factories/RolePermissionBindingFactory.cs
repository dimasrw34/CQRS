using System;
using System.Collections.Generic;
using InTouch.UserService.Core;


namespace InTouch.UserService.Domain;

public class RolePermissionBindingFactory
{
    public static RolePermissionBinding Create(Guid roleid, Guid permissionid)
        => new(roleid, permissionid);
}