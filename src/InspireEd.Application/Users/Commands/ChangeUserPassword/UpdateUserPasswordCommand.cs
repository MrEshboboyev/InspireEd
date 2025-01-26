using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Users.Commands.ChangeUserPassword;

public sealed record UpdateUserPasswordCommand(
    Guid UserId,
    string NewPassword) : ICommand;
