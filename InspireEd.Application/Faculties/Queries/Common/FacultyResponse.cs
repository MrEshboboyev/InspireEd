namespace InspireEd.Application.Faculties.Queries.Common;

public sealed record FacultyResponse(
    Guid FacultyId,
    string FacultyName,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc);