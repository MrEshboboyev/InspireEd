using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Commands.CreateFaculty;

public sealed record CreateFacultyCommand(
    string FacultyName) : ICommand;