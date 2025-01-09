using FluentValidation;

namespace InspireEd.Application.Classes.Attendances.Commands.UpdateAttendance;

internal class UpdateAttendanceCommandValidator : AbstractValidator<UpdateAttendanceCommand>
{
    public UpdateAttendanceCommandValidator()
    {
        RuleFor(x => x.ClassId).NotEmpty();
        RuleFor(x => x.AttendanceId).NotEmpty();
        RuleFor(x => x.AttendanceStatus).IsInEnum();
        RuleFor(x => x.Notes).MaximumLength(500);
    }
}