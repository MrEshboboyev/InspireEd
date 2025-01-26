using FluentValidation;

namespace InspireEd.Application.Subjects.Commands.DeleteSubject;

internal class DeleteSubjectCommandValidator : AbstractValidator<DeleteSubjectCommand>
{
    public DeleteSubjectCommandValidator()
    {
        RuleFor(x => x.SubjectId).NotEmpty();
    }
}