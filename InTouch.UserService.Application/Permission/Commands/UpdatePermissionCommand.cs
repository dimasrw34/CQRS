using System;
using Ardalis.Result;
using MediatR;

namespace InTouch.UserService.Application;

/// <summary>
/// Комманда на обновление permission
/// </summary>
/// <param name="Id">Уникальный идентификатор</param>
/// <param name="Title">Название permission</param>
public sealed record UpdatePermissionCommand (Guid Id, string Title)
: IRequest<Result>;
