namespace InspireEd.Application.Faculties.Queries.Common;

public sealed record FacultyResponse(
    Guid Id,
    string Name,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc,
    IEnumerable<Guid> DepartmentHeadIds,
    IEnumerable<GroupResponse> Groups);