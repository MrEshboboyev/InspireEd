using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Users.Commands.AssignRoleToUser;

public sealed record AssignRoleToUserCommand(
    Guid UserId,
    int RoleId) : ICommand;