using FluentValidation;

namespace InspireEd.Application.Users.Commands.RemoveRoleFromUser;

public class RemoveRoleFromUserCommandValidator : AbstractValidator<RemoveRoleFromUserCommand>
{
    public RemoveRoleFromUserCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty().WithMessage("UserId is required.");
        
        RuleFor(command => command.RoleId)
            .NotEmpty().WithMessage("RoleId is required.");
    }
}