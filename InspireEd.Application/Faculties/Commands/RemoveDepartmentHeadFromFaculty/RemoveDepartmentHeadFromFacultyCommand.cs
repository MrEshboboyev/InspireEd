using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Commands.RemoveDepartmentHeadFromFaculty;

public sealed record RemoveDepartmentHeadFromFacultyCommand(
    Guid FacultyId,
    Guid DepartmentHeadId) : ICommand;