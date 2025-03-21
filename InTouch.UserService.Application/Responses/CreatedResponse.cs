using System;
using Ardalis.Result;
using MediatR;
using InTouch.UserService.Core;

namespace InTouch.Application;

/// <summary>
/// Согласно паттерну SQRS, Command не должен ничего и никогда возвращать,
/// так как для извлечения любых данных есть Query. Но на реальных проектах,
/// нам нужно обязательно быть уверенными, что мы извлекаем ИД именно того объекта,
/// который мы создали. Поэтому для обхода "теории" мы подсовываем объект, который
/// как-бы не влияет на поведение объекта Command. 
/// </summary>
/// <param name="id">Идентификатор создаваемого объекта</param>
public sealed class CreatedResponse (Guid id) : IResponse
{
    public Guid Id { get; } = id;
}