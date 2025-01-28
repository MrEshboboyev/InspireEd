using FluentValidation;
using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Subjects.Commands.ChangeSubjectCode;

internal class ChangeSubjectCodeCommandValidator : AbstractValidator<ChangeSubjectCodeCommand>
{
    public ChangeSubjectCodeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.NewCode).NotEmpty();
    }
}
