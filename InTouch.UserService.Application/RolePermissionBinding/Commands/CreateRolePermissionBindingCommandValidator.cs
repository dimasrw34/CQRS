using FluentValidation;

namespace InTouch.UserService.Application;


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