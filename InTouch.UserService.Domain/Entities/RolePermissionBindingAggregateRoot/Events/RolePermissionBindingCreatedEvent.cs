using System;

namespace InTouch.UserService.Domain;

public sealed class RolePermissionBindingCreatedEvent (Guid id, Role role, PermissionIT permission)
    : RolePermissionBindingBaseEvent(id, role, permission);