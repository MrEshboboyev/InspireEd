using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Commands.Groups.UpdateGroup;

public sealed record UpdateGroupCommand(
    Guid FacultyId,
    Guid GroupId,
    string GroupName) : ICommand;