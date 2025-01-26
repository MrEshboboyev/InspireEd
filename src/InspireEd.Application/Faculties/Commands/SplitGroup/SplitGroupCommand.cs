using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Commands.SplitGroup;

public sealed record SplitGroupCommand(
    Guid FacultyId,
    Guid GroupId,
    int NumberOfGroups) : ICommand;