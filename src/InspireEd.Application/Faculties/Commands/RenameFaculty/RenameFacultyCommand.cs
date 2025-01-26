using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Commands.RenameFaculty;

public sealed record RenameFacultyCommand(
    Guid FacultyId,
    string FacultyName) : ICommand;