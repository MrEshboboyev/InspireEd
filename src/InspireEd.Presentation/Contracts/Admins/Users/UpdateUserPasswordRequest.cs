namespace InspireEd.Presentation.Contracts.Admins.Users;

public sealed record UpdateUserPasswordRequest(
    string NewPassword);