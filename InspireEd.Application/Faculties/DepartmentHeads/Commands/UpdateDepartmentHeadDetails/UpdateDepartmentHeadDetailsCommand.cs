using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.DepartmentHeads.Commands.UpdateDepartmentHeadDetails;

public sealed record UpdateDepartmentHeadDetailsCommand(
    Guid FacultyId,
    Guid DepartmentHeadId,
    string FirstName,
    string LastName,
    string Email) : ICommand;