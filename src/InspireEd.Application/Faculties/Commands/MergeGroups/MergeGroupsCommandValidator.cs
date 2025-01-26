using FluentValidation;

namespace InspireEd.Application.Faculties.Commands.MergeGroups;

internal class MergeGroupsCommandValidator : AbstractValidator<MergeGroupsCommand>
{
    public MergeGroupsCommandValidator()
    {
        RuleFor(command => command.FacultyId)
            .NotEmpty().WithMessage("FacultyId is required.");

        RuleFor(command => command.GroupIds)
            .NotEmpty().WithMessage("GroupIds are required.")
            .Must(ids => ids.Count > 1)
            .WithMessage("At least two GroupIds are required to merge groups.");
    }
}