using System;

namespace InTouch.UserService.Core;

public class EventStore : BaseEvent, IEntity<Guid>
{
    /// <summary>
    /// Конструктор по умолчанию для Entity Framework или других фреймворков ORM.
    /// </summary>
    public EventStore() { }

    public EventStore(Guid aggregateID, string messageType, string data)
    {
        AggregateID = aggregateID;
        MessageType = messageType; 
        Data = data;
    }


    /// <summary>
    /// Получает или задает идентификатор.
    /// </summary>
    public Guid Id { get; private init; } = Guid.NewGuid();

    /// <summary>
    /// Получает или задает данные.
    /// </summary>
    public string Data { get; private init; }
}