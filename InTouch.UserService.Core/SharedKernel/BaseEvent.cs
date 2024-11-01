using System;

namespace InTouch.UserService.Core;

/// <summary>
/// Представляет собой базовое событие.
/// </summary>
public abstract class BaseEvent : MediatR.INotification
{
    /// <summary>
    /// Получает тип сообщения.
    /// </summary>
    public string MessageType { get; protected init; }
    
    /// <summary>
    /// Получает AggregateRoot идентификатор.
    /// </summary>
    public Guid AggregateID { get; protected init; }
    
    /// <summary>
    /// Получает дату и время, когда произошло событие.
    /// </summary>
    public DateTime OccuredOn { get; protected init; } = DateTime.Now;
}