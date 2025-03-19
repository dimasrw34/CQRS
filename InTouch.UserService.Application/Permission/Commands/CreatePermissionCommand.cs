using Ardalis.Result;
using MediatR;

namespace InTouch.Application;

public sealed record CreatePermissionCommand(string Title)
    : IRequest<Result<CreatedResponse>>;
