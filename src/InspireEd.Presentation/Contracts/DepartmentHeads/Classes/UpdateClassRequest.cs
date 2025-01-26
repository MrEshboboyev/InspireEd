using InspireEd.Domain.Classes.Enums;

namespace InspireEd.Presentation.Contracts.DepartmentHeads.Classes;

public sealed record UpdateClassRequest(
    Guid SubjectId,
    Guid TeacherId,
    ClassType Type,
    DateTime ScheduledDate);