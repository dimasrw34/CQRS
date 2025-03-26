using Ardalis.Result;
using MediatR;

namespace InTouch.UserService.Application;

/// <summary>
/// Создает permission 
/// </summary>
/// <param name="Title">Название пермишена</param>
public sealed record CreatePermissionCommand (string Title)
    : IRequest<Result<CreatedResponse>>;
