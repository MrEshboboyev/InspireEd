using FluentValidation;

namespace InspireEd.Application.Classes.Commands.CreateAttendances;

internal class CreateAttendancesCommandValidator : AbstractValidator<CreateAttendancesCommand>
{
    public CreateAttendancesCommandValidator()
    {
        RuleFor(x => x.ClassId).NotEmpty();
        RuleFor(x => x.Attendances).NotEmpty();

        RuleForEach(x => x.Attendances)
            .ChildRules(attendance =>
            {
                attendance.RuleFor(a => a.StudentId).NotEmpty();
                attendance.RuleFor(a => a.Status).IsInEnum();
                attendance.RuleFor(a => a.Notes).MaximumLength(500);
            });
    }
}