using FluentValidation;

namespace InspireEd.Application.Users.Commands.AssignRoleToUser;

internal class AssignRoleToUserCommandValidator : AbstractValidator<AssignRoleToUserCommand>
{
    public AssignRoleToUserCommandValidator()
    {
        RuleFor(command => command.UserId)
            .NotEmpty().WithMessage("UserId is required.");
        
        RuleFor(command => command.RoleId)
            .NotEmpty().WithMessage("RoleId is required.");
    }
}