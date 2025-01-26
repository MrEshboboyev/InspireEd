using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Users.Commands.UpdateUser;

public sealed record UpdateUserCommand(
    Guid UserId,
    string FirstName,
    string LastName) : ICommand;