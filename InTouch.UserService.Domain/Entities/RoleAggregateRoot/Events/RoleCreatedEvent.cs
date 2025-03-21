using System;
using InTouch.UserService.Core;

namespace InTouch.UserService.Domain;

public sealed class RoleCreatedEvent(Guid id, string title)
    : RoleBaseEvent(id, title);
