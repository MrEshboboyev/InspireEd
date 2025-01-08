using FluentValidation;

namespace InspireEd.Application.Classes.Commands.CreateClass;

internal class CreateClassCommandValidator : AbstractValidator<CreateClassCommand>
{
    public CreateClassCommandValidator()
    {
        RuleFor(obj => obj.SubjectId)
            .NotEmpty().WithMessage("SubjectId cannot be empty.");

        RuleFor(obj => obj.TeacherId)
            .NotEmpty().WithMessage("TeacherId cannot be empty.");

        RuleFor(obj => obj.ClassType)
            .NotEmpty().WithMessage("ClassType cannot be empty.")
            .IsInEnum().WithMessage("Invalid ClassType value.");

        RuleFor(obj => obj.GroupIds)
            .NotEmpty().WithMessage("GroupIds cannot be empty.")
            .Must(groupIds => groupIds is
            {
                Count: > 0
            })
            .WithMessage("GroupIds must contain at least one group.");

        RuleFor(obj => obj.ScheduledDate)
            .GreaterThan(DateTime.Now).WithMessage("ScheduledDate must be in the future.");
    }
}