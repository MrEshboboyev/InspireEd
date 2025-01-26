using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Commands.AddGroupToFaculty;

public sealed record AddGroupToFacultyCommand(
    Guid FacultyId,
    string GroupName) : ICommand;