using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Commands.DepartmentHeads.AddDepartmentHead;

public sealed record AddDepartmentHeadCommand(
    Guid FacultyId,
    Guid DepartmentHeadId) : ICommand;