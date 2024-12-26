namespace InspireEd.Application.Faculties.Queries.Common;

public sealed record FacultyListResponse(
    IReadOnlyCollection<FacultyResponse> Faculties);