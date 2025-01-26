using FluentValidation;

namespace InspireEd.Application.Subjects.Commands.UpdateSubject;

public class UpdateSubjectCommandValidator : AbstractValidator<UpdateSubjectCommand>
{
    public UpdateSubjectCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Code).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Credit).GreaterThan(0);
    }
}