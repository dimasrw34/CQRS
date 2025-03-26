using Ardalis.Result;
using InTouch.UserService.Domain;
using MediatR;

namespace InTouch.UserService.Application;


public sealed record CreateUserCommand(string Login, string Password, string FirstName, string LastName, string Email, string Phone)
    : IRequest<Result<CreatedResponse>>;