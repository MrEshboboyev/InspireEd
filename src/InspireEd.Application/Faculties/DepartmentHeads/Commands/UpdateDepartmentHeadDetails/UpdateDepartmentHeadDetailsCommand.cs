using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.DepartmentHeads.Commands.UpdateDepartmentHeadDetails;

public sealed record UpdateDepartmentHeadDetailsCommand(
    Guid DepartmentHeadId,
    string FirstName,
    string LastName,
    string Email,
    string Password) : ICommand;