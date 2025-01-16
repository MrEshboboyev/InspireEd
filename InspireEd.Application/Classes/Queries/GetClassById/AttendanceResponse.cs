using InspireEd.Domain.Classes.Enums;

namespace InspireEd.Application.Classes.Queries.GetClassById;

public sealed record AttendanceResponse(
    Guid Id,
    Guid StudentId,
    AttendanceStatus Status,
    string Notes,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc);