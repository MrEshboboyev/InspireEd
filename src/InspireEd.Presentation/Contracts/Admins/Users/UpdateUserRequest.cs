namespace InspireEd.Presentation.Contracts.Admins.Users;

public sealed record UpdateUserRequest(
    string FirstName,
    string LastName);