using System;
using Ardalis.Result;
using MediatR;

namespace InTouch.UserService.Application;

public sealed record DeleteRoleCommand(Guid Id): IRequest<Result>;
