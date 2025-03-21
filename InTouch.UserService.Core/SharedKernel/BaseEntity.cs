using System;
using FDS.UuidV7.NetCore;
using System.Collections.Generic;

namespace InTouch.UserService.Core;

/// <summary>
/// Представляет абстрактный базовый класс сущностей.
/// </summary>
public abstract class BaseEntity : IEntity<Guid>
{
    /// <summary>
    /// Брокер событий
    /// </summary>
    private readonly List<BaseEvent> _domainEvents = [];

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="BaseEntity"/> class.
    /// </summary>
    protected BaseEntity() => Id = UuidV7.Generate();
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="BaseEntity"/> с указанным идентификатором.
    /// </summary>
    /// <param name="id">Уникальный идентификатор.</param>
    protected BaseEntity(Guid id) => Id = id;

    /// <summary>
    /// Получает уникальный идентификатор этой сущности.
    /// </summary>   
    public Guid Id { get; private init; }
    
    /// <summary>
    /// Получает события домена, связанные с этой сущностью.
    /// </summary>
    public IEnumerable<BaseEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>
    /// Добавляет событие домена к сущности.
    /// </summary>
    /// <param name="domainEvent"></param>
    protected void AddDomainEvent(BaseEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    /// <summary>
    /// Очищает все события домена, связанные с этой сущностью.
    /// </summary>
    public void ClearDomainEvents() => _domainEvents.Clear();

}