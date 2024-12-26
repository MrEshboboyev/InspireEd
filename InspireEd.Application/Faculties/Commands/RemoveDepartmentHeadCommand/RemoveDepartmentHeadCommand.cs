using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Commands.RemoveDepartmentHeadCommand;

public sealed record RemoveDepartmentHeadCommand(
    Guid FacultyId,
    Guid DepartmentHeadId) : ICommand;