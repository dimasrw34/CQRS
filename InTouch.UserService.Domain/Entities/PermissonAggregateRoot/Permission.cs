using InTouch.UserService.Core;

namespace InTouch.UserService.Domain;

public sealed class Permission  : BaseEntity, IAggregateRoot
{
    public Permission() {}

    /// <summary>
    /// Инициализирует новый экземпляр класса Permission
    /// </summary>
    /// <param name="title"></param>
    public Permission(string title)
    {
        Title = title;
        AddDomainEvent(new PermissionCreatedEvent(Id, Title));
    }
    
    /// <summary>
    /// Получаем название пермишена 
    /// </summary>
    public string Title { get; }
}