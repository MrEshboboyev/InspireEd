using FluentValidation;

namespace InspireEd.Application.Classes.Commands.RescheduleClass;

public class RescheduleClassCommandValidator : AbstractValidator<RescheduleClassCommand>
{
    public RescheduleClassCommandValidator()
    {
        RuleFor(x => x.ClassId).NotEmpty();
        RuleFor(x => x.NewScheduledDate).NotEmpty()
            .GreaterThan(DateTime.UtcNow);
    }
}