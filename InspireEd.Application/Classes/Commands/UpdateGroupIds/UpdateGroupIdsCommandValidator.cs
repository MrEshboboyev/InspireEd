using FluentValidation;

namespace InspireEd.Application.Classes.Commands.UpdateGroupIds;

public class UpdateGroupIdsCommandValidator : AbstractValidator<UpdateGroupIdsCommand>
{
    public UpdateGroupIdsCommandValidator()
    {
        RuleFor(x => x.ClassId).NotEmpty();
        RuleFor(x => x.GroupIds).NotEmpty();
        RuleForEach(x => x.GroupIds).NotEmpty();
    }
}