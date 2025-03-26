using Ardalis.Result;
using MediatR;

namespace InTouch.UserService.Application;

public sealed record CreateRoleCommand(string Title) : IRequest<Result<CreatedResponse>>;