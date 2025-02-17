using FluentValidation;

namespace InspireEd.Application.Classes.Commands.UpdateClassGroups;

internal class UpdateClassGroupsCommandValidator : AbstractValidator<UpdateClassGroupsCommand>
{
    public UpdateClassGroupsCommandValidator()
    {
        RuleFor(x => x.ClassId).NotEmpty();
        RuleFor(x => x.GroupIds).NotEmpty();
        RuleForEach(x => x.GroupIds).NotEmpty();
    }
}