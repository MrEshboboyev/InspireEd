using FluentValidation;

namespace InspireEd.Application.Users.Commands.DeleteUser;

internal class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}