using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Commands.DeleteFaculty;

public sealed record DeleteFacultyCommand(
    Guid FacultyId) : ICommand;