using System;
using InTouch.UserService.Core;

namespace InTouch.UserService.Domain;

public sealed class PermissionCreatedEvent(Guid id, string title)
    : PermissionBaseEvent(id,title);
