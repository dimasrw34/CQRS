using System.Data;
using FluentValidation;

namespace InTouch.UserService.Application;


public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();
    }
}