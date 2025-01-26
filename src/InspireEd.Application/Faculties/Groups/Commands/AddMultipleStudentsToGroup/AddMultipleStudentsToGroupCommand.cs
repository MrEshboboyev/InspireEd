using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Groups.Commands.AddMultipleStudentsToGroup;

public sealed record AddMultipleStudentsToGroupCommand(
    Guid FacultyId,
    Guid GroupId,
    List<(
        string FirstName,
        string LastName,
        string Email,
        string Password)> Students) : ICommand;