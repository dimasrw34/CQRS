using FluentValidation;

namespace InTouch.Application;

public sealed class CreateRolePermissionBindingCommandValidator: AbstractValidator<CreateRolePermissionBindingCommand>
{
    public CreateRolePermissionBindingCommandValidator()
    {
        RuleFor(command => command.PermissionId)
            .NotEmpty();
            
        RuleFor(command => command.RoleId)
            .NotEmpty();
    }
}