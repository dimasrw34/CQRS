using System;
using FluentValidation;

namespace InTouch.UserService.Application;

public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
{
    public UpdateRoleCommandValidator()
    {
         RuleFor(command => command.Id)
            .NotEmpty();
            
         RuleFor(command => command.Title)
            .NotEmpty()
            .MaximumLength(100);
    }
}
