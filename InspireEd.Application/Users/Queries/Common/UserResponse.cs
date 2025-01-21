namespace InspireEd.Application.Users.Queries.Common;

public sealed record UserResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    DateTime CreatedOnUtc,
    DateTime? ModifiedOnUtc);