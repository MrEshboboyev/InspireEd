using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Groups.Commands.UpdateGroup;

public sealed record UpdateGroupCommand(
    Guid FacultyId,
    Guid GroupId,
    string GroupName) : ICommand;