using InspireEd.Domain.Classes.Enums;

namespace InspireEd.Presentation.Contracts.Teachers.Classes;

public sealed record CreateAttendancesRequest(
    List<(
        Guid StudentId,
        AttendanceStatus Status,
        string Notes)> Attendances);