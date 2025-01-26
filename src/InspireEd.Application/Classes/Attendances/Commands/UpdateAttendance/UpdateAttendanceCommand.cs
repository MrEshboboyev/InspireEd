using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Classes.Enums;

namespace InspireEd.Application.Classes.Attendances.Commands.UpdateAttendance;

public sealed record UpdateAttendanceCommand(
    Guid ClassId,
    Guid AttendanceId,
    AttendanceStatus AttendanceStatus,
    string Notes) : ICommand;