using FluentValidation;

namespace InTouch.UserService.Application;

public sealed class DeleteRolePermissionBindingCommandValidator : AbstractValidator<DeleteRolePermissionBindingCommand>
{
    public DeleteRolePermissionBindingCommandValidator()
    {
        RuleFor(command => command.PermissionId)
        .NotEmpty();
            
        RuleFor(command => command.RoleId)
        .NotEmpty();
    }
}