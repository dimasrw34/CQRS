using FluentValidation;

namespace InTouch.Application;

public sealed class CreateUserCommandValidator: AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(command => command.FirstName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.LastName)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(command => command.Login)
            .NotEmpty()
            .MaximumLength(254);
        RuleFor(command => command.Login)
            .NotEmpty()
            .MaximumLength(254)
            .EmailAddress();
    }
}