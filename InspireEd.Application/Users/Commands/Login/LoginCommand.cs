using InspireEd.Application.Abstractions.Messaging;

namespace InspireEd.Application.Users.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<string>;
