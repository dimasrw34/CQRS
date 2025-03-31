using FluentValidation;

namespace InTouch.UserService.Application;

public class UpdatePermissionCommandValidator : AbstractValidator<UpdatePermissionCommand>
{
    public UpdatePermissionCommandValidator() 
    {
        RuleFor(command=>command.Id)
            .NotEmpty();

        RuleFor(command => command.Title)
            .NotEmpty()
            .MaximumLength(40);   
    }
}