namespace InspireEd.Presentation.Contracts.Users;

public record AddMultipleUsersRequest(
    List<CreateUserRequest> Users);