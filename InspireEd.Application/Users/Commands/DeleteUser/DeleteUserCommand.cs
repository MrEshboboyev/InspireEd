using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Users.Commands.DeleteUser;

public sealed record DeleteUserCommand(
    Guid UserId) : ICommand;