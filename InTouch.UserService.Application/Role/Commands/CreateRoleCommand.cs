using Ardalis.Result;
using MediatR;

namespace InTouch.Application;

public sealed record CreateRoleCommand(string Title) : IRequest<Result<CreatedResponse>>;