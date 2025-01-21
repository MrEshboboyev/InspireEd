using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Users.Commands.ChangeUserPassword;

public sealed record ChangeUserPasswordCommand(
    Guid UserId,
    string OldPassword,
    string NewPassword) : ICommand;
