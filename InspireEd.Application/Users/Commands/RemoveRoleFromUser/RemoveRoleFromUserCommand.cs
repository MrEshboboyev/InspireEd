using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Users.Commands.RemoveRoleFromUser;

public sealed record RemoveRoleFromUserCommand(
    Guid UserId,
    int RoleId) : ICommand;