namespace InspireEd.Application.Subjects.Queries.Common;

public sealed record SubjectResponse(
    Guid Id,
    string Name,
    string Code,
    int Credit,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc);