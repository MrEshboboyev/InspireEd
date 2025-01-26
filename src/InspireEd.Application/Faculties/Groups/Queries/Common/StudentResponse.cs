namespace InspireEd.Application.Faculties.Groups.Queries.Common;

public sealed record StudentResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc);