using FluentValidation;

namespace InspireEd.Application.Subjects.Commands.RenameSubject;

internal class RenameSubjectCommandValidator : AbstractValidator<RenameSubjectCommand>
{
    public RenameSubjectCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.NewName).NotEmpty().MaximumLength(100);
    }
}