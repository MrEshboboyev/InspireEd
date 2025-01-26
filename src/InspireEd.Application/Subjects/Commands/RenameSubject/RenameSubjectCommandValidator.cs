using FluentValidation;

namespace InspireEd.Application.Subjects.Commands.RenameSubject;

public class RenameSubjectCommandValidator : AbstractValidator<RenameSubjectCommand>
{
    public RenameSubjectCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.NewName).NotEmpty().MaximumLength(100);
    }
}