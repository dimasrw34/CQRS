using System;
using InTouch.UserService.Core;

namespace InTouch.UserService.Domain;

public class RoleBaseEvent : BaseEvent
{
    protected RoleBaseEvent(Guid id, string title)
    {
        Id = id;
        Title = title;
    }

    public Guid Id { get; private init; }
    public string Title { get; private init; }   
}
