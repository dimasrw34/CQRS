using System;
using Ardalis.Result;
using MediatR;

namespace InTouch.UserService.Application;

public sealed record UpdateRoleCommand (Guid Id, string Title) : IRequest<Result>;

