using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Classes.Attendances.Commands.DeleteAttendance;

public sealed record DeleteAttendanceCommand(
    Guid ClassId,
    Guid AttendanceId) : ICommand;