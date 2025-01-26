using FluentValidation;

namespace InspireEd.Application.Classes.Commands.DeleteClass;

internal class DeleteClassCommandValidator : AbstractValidator<DeleteClassCommand>
{
    public DeleteClassCommandValidator()
    {
        RuleFor(x => x.ClassId).NotEmpty();
    }
}