using FluentValidation;

namespace InTouch.UserService.Application;

/// <summary>
/// Валидатор CreatePermissionCommand
/// </summary>
public class CreatePermissionCommandValidator : AbstractValidator<CreatePermissionCommand>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    public CreatePermissionCommandValidator()
    {
        RuleFor(command => command.Title)
            .NotEmpty()
            .MaximumLength(40);
    }
}