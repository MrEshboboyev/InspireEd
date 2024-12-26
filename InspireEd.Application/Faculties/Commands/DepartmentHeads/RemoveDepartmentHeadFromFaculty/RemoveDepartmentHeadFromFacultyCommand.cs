using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Commands.DepartmentHeads.RemoveDepartmentHead;

public sealed record RemoveDepartmentHeadFromFacultyCommand(
    Guid FacultyId,
    Guid DepartmentHeadId) : ICommand;