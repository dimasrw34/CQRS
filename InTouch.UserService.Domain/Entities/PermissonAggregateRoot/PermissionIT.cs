using InTouch.UserService.Core;


namespace InTouch.UserService.Domain;

public sealed class PermissionIT  : BaseEntity, IAggregateRoot
{
    public PermissionIT() {}

    /// <summary>
    /// Инициализирует новый экземпляр класса Permission
    /// </summary>
    /// <param name="title"></param>
    public PermissionIT(string title)
    {
        Title = title;
        AddDomainEvent(new PermissionCreatedEvent(Id, Title));
    }
    
    /// <summary>
    /// Получаем название пермишена 
    /// </summary>
    public string Title { get; }

    public override string ToString() => Title;
}