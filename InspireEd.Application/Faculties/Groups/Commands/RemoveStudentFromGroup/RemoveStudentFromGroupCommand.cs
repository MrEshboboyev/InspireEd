using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Groups.Commands.RemoveStudentFromGroup;

public sealed record RemoveStudentFromGroupCommand(
    Guid FacultyId,
    Guid GroupId,
    Guid StudentId) : ICommand;