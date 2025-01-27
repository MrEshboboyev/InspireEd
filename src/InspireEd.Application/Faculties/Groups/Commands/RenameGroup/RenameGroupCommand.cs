using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Groups.Commands.RenameGroup;

public sealed record RenameGroupCommand(
    Guid FacultyId,
    Guid GroupId,
    string GroupName) : ICommand;