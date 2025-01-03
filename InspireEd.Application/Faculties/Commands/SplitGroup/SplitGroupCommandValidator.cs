using FluentValidation;

namespace InspireEd.Application.Faculties.Commands.SplitGroup;

internal class SplitGroupCommandValidator : AbstractValidator<SplitGroupCommand>
{
    public SplitGroupCommandValidator()
    {
        RuleFor(command => command.FacultyId)
            .NotEmpty().WithMessage("FacultyId is required.");

        RuleFor(command => command.GroupId)
            .NotEmpty().WithMessage("GroupId is required.");

        RuleFor(command => command.NumberOfGroups)
            .GreaterThan(1).WithMessage("NumberOfGroups must be greater than 1.");
    }
}