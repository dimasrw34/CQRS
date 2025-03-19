using System;

namespace InTouch.UserService.Domain;

public sealed class PermissionCreatedEvent(Guid id, string title)
    : PermissionBaseEvent(id,title);
