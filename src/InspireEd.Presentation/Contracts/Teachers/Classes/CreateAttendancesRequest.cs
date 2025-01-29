using InspireEd.Domain.Classes.Enums;

namespace InspireEd.Presentation.Contracts.Teachers.Classes;

public sealed record CreateAttendancesRequest(
    List<CreateAttendanceRequest> Attendances);
        
public sealed record CreateAttendanceRequest(
    Guid StudentId,
    AttendanceStatus Status,
    string Notes);