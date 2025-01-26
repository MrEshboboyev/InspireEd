using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Groups.Commands.AddStudentToGroup;

public sealed record AddStudentToGroupCommand(
    Guid FacultyId,
    Guid GroupId,
    string StudentFirstName,
    string StudentLastName,
    string StudentEmail,
    string StudentPassword) : ICommand;