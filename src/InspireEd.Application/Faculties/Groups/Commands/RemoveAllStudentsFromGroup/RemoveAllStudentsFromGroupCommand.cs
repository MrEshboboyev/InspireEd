using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Groups.Commands.RemoveAllStudentsFromGroup;

public sealed record RemoveAllStudentsFromGroupCommand(
    Guid FacultyId,
    Guid GroupId) : ICommand;