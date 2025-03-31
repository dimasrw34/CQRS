using InTouch.UserService.Core;

namespace InTouch.UserService.Domain;

public class Role : BaseEntity, IAggregateRoot
{
    public Role()
    {
    }

    public Role(string title)
    {
        Title = title;
        AddDomainEvent(new RoleCreatedEvent(Id, Title));
    }

    public string Title { get; }

    public override string ToString() => Title;
}