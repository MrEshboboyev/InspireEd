using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Classes.Commands.UpdateGroupIds;

public sealed record UpdateGroupIdsCommand(
    Guid ClassId,
    List<Guid> GroupIds) : ICommand;