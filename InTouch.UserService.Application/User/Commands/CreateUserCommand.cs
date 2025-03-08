using Ardalis.Result;
using MediatR;

namespace InTouch.Application;

public sealed record CreateUserCommand(string Email, string Password, string FirstName, string LastName, string Phone)
    : IRequest<Result<CreatedUserResponse>>;