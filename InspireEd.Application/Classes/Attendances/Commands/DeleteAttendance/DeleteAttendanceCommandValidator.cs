using FluentValidation;

namespace InspireEd.Application.Classes.Attendances.Commands.DeleteAttendance;

internal class DeleteAttendanceCommandValidator : AbstractValidator<DeleteAttendanceCommand>
{
    public DeleteAttendanceCommandValidator()
    {
        RuleFor(x => x.ClassId).NotEmpty();
        RuleFor(x => x.AttendanceId).NotEmpty();
    }
}