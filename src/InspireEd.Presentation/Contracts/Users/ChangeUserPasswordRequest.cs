namespace InspireEd.Presentation.Contracts.Users;

public sealed record ChangeUserPasswordRequest(
    string OldPassword,
    string NewPassword);