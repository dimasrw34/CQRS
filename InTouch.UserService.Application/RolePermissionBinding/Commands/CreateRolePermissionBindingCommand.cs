using System;
using Ardalis.Result;
using MediatR;
using InTouch.UserService.Core;

namespace InTouch.Application;
/// <summary>
/// Добавление пермишену роли
/// </summary>
/// <param name="RoleId"></param>
/// <param name="PermisionId"></param>
public sealed record CreateRolePermissionBindingCommand(Guid RoleId, Guid PermissionId)
    : IRequest<Result<CreatedResponse>>;