namespace InspireEd.Application.Faculties.Queries.Common;

public sealed record GroupResponse(
    Guid Id,
    string Name,
    IEnumerable<Guid> StudentIds,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc);