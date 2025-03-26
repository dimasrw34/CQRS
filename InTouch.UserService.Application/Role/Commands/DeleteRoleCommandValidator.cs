using System;
using FluentValidation;

namespace InTouch.UserService.Application;

public sealed class DeleteRoleCommandValidator : AbstractValidator<DeleteRoleCommand>
{
    public DeleteRoleCommandValidator()
    {
          RuleFor(command => command.Id)
            .NotEmpty();
    }
}
