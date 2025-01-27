namespace InspireEd.Application.Faculties.Queries.Common;

public sealed record DepartmentHeadResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc);
