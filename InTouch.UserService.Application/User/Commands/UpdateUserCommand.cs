using System;
using Ardalis.Result;
using MediatR;

namespace InTouch.UserService.Application;


public sealed class UpdateUserCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    
    public string Email { get; set; }
}