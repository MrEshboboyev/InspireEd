using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Users.Commands.CreateUser;

public sealed record CreateUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName) : ICommand<Guid>;
