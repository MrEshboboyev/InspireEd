using FluentValidation;

namespace InspireEd.Application.Faculties.Groups.Commands.TransferStudentBetweenGroups;

internal class TransferStudentBetweenGroupsCommandValidator : AbstractValidator<TransferStudentBetweenGroupsCommand>
{
    public TransferStudentBetweenGroupsCommandValidator()
    {
        RuleFor(command => command.FacultyId)
            .NotEmpty().WithMessage("FacultyId is required.");

        RuleFor(command => command.SourceGroupId)
            .NotEmpty().WithMessage("SourceGroupId is required.");

        RuleFor(command => command.TargetGroupId)
            .NotEmpty().WithMessage("TargetGroupId is required.");

        RuleFor(command => command.StudentId)
            .NotEmpty().WithMessage("StudentId is required.");
    }
}