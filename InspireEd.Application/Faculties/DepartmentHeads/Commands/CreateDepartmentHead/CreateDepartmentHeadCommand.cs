using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.DepartmentHeads.Commands.CreateDepartmentHead;

public sealed record CreateDepartmentHeadCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password) : ICommand;