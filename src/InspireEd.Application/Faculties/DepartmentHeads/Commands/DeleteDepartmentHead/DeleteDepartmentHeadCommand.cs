using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.DepartmentHeads.Commands.DeleteDepartmentHead;

public sealed record DeleteDepartmentHeadCommand(
    Guid DepartmentHeadId) : ICommand;