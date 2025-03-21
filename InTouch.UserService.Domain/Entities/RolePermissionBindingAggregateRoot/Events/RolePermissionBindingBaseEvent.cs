using System;
using InTouch.UserService.Core;

namespace InTouch.UserService.Domain;

public abstract class RolePermissionBindingBaseEvent: BaseEvent
{
    protected RolePermissionBindingBaseEvent(Guid id, Role role, Permission permission)
    {
        Id = id;
        Role = role;
        Permission = permission;
    }
    public Guid Id { get; private init; }
    public Role Role { get; private init; }
    public Permission Permission { get; private init; }
}