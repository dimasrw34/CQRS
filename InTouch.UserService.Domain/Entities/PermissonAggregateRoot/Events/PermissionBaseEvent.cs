using System;
using InTouch.UserService.Core;

namespace InTouch.UserService.Domain;

public abstract class PermissionBaseEvent: BaseEvent
{
    protected PermissionBaseEvent(Guid id, string title)
    {
        Id = id;
        Title = title;
    }

    public Guid Id { get; private init; }
    public string Title { get; private init; }
}