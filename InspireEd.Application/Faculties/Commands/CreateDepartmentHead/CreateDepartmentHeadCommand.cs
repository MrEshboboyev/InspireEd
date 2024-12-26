using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Faculties.Commands.CreateDepartmentHead;

public sealed record CreateDepartmentHeadCommand(
    string Email,
    string FirstName,
    string LastName,
    string Password) : ICommand;