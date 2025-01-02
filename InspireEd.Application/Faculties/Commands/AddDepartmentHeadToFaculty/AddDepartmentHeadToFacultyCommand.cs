using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Commands.AddDepartmentHeadToFaculty;

public sealed record AddDepartmentHeadToFacultyCommand(
    Guid FacultyId,
    Guid DepartmentHeadId) : ICommand;