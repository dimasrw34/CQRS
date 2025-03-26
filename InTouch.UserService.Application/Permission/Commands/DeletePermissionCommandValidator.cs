using FluentValidation;

namespace InTouch.UserService.Application;

public sealed class DeletePermissionCommandValidator : AbstractValidator<DeletePermissionCommand>
{
    public DeletePermissionCommandValidator() => RuleFor(command => command.Id).NotEmpty();
}