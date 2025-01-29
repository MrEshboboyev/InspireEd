using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Classes.Commands.UpdateClassGroups;

public sealed record UpdateClassGroupsCommand(
    Guid ClassId,
    List<Guid> GroupIds) : ICommand;