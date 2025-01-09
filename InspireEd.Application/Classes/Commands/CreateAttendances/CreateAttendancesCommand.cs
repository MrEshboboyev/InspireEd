using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Classes.Enums;

namespace InspireEd.Application.Classes.Commands.CreateAttendances;

public sealed record CreateAttendancesCommand(
    Guid ClassId,
    List<(
        Guid StudentId,
        AttendanceStatus Status,
        string Notes)> Attendances) : ICommand;