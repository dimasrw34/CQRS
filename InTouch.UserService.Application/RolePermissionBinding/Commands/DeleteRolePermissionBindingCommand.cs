using System;
using Ardalis.Result;
using MediatR;

namespace InTouch.UserService.Application;

public sealed record DeleteRolePermissionBindingCommand(Guid RoleId, Guid PermissionId) : IRequest<Result>;