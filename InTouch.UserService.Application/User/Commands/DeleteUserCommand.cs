using System;
using Ardalis.Result;
using MediatR;

namespace InTouch.UserService.Application;


public class DeleteUserCommand(Guid id) :IRequest<Result>
{
    public Guid Id { get; } = id;
}