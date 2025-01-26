using InspireEd.Domain.Classes.Enums;

namespace InspireEd.Presentation.Contracts.DepartmentHeads.Classes;

public sealed record CreateClassRequest(
    Guid SubjectId,
    Guid TeacherId,
    ClassType ClassType,
    List<Guid> GroupIds,
    DateTime ScheduledDate);
