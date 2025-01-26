using FluentValidation;

namespace InspireEd.Application.Subjects.Commands.ChangeSubjectCredit;

public class ChangeSubjectCreditCommandValidator : AbstractValidator<ChangeSubjectCreditCommand>
{
    public ChangeSubjectCreditCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.NewCredit).GreaterThan(0);
    }
}