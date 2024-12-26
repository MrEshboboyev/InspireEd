using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Commands.DepartmentHeads.DeleteDepartmentHeadCommand;

public sealed record DeleteDepartmentHeadCommand(
    Guid DepartmentHeadId) : ICommand;