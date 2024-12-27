using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Commands.Groups.RemoveGroupFromFaculty;

public sealed record RemoveGroupFromFacultyCommand(
    Guid FacultyId,
    Guid GroupId) : ICommand;