using InspireEd.Domain.Classes.Enums;

namespace InspireEd.Presentation.Contracts.DepartmentHeads.Classes.Attendances;

public sealed record UpdateAttendanceRequest(
    AttendanceStatus AttendanceStatus,
    string Notes);