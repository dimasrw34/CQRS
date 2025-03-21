using System;
using Ardalis.Result;
using InTouch.UserService.Core;
using MediatR;

namespace InTouch.Application;

public class DeleteUserCommand(Guid id) :IRequest<Result>
{
    public Guid Id { get; } = id;
}