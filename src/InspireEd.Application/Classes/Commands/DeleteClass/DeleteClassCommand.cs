using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Classes.Commands.DeleteClass;

public sealed record DeleteClassCommand(
    Guid ClassId) : ICommand;