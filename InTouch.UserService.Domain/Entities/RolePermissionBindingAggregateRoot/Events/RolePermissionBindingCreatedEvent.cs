using System;
using InTouch.UserService.Core;

namespace InTouch.UserService.Domain;

public sealed class RolePermissionBindingCreatedEvent (Guid id, Role role, Permission permission)
    : RolePermissionBindingBaseEvent(id, role, permission);