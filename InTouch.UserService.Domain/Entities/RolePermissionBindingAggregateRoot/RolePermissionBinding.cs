using System;
using System.Collections.Generic;
using InTouch.UserService.Core;


namespace InTouch.UserService.Domain;

public sealed class RolePermissionBinding : BaseEntity, IAggregateRoot
{
    public RolePermissionBinding() { }

    public RolePermissionBinding(Guid role, Guid permission)
    {
        Role = role;
        Permission = permission;
    }
    public Guid Role { get; }
    public Guid Permission { get; }
}