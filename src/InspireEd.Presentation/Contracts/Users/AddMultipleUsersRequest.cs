namespace InspireEd.Presentation.Contracts.Users;

public sealed record AddMultipleUsersRequest(
    List<CreateUserRequest> Users);