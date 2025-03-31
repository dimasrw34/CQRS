using System;
using Ardalis.Result;
using MediatR;

namespace InTouch.UserService.Application;

public sealed record DeletePermissionCommand (Guid Id) : IRequest<Result>;

