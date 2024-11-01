using Ardalis.Result;
using MediatR;

namespace InTouch.Application;

public sealed class CreateUserCommand : IRequest<Result<CreatedUserResponse>>
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Phone { get; set; }
}