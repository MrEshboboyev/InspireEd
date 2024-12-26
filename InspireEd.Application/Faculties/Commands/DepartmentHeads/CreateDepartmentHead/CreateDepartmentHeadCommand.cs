using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Commands.DepartmentHeads.CreateDepartmentHead;

public sealed record CreateDepartmentHeadCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password) : ICommand;