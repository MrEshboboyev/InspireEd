using InspireEd.Domain.Classes.Enums;

namespace InspireEd.Application.Classes.Queries.Common;

public sealed record ClassResponse(
    Guid Id,
    Guid SubjectId,
    Guid TeacherId,
    ClassType Type,
    IReadOnlyCollection<Guid> GroupIds,
    DateTime ScheduledDate,
    IReadOnlyCollection<AttendanceResponse> Attendances);