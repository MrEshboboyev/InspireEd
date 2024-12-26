using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Commands.UpdateFaculty;

public sealed record UpdateFacultyCommand(
    Guid FacultyId,
    string FacultyName) : ICommand;