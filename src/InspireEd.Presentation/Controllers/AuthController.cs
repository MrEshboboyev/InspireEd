using InspireEd.Application.Users.Commands.ChangeUserPassword;
using InspireEd.Application.Users.Commands.CreateUser;
using InspireEd.Application.Users.Commands.Login;
using InspireEd.Application.Users.Queries.GetUserById;
using InspireEd.Domain.Users.Enums;
using InspireEd.Infrastructure.Authentication;
using InspireEd.Presentation.Abstractions;
using InspireEd.Presentation.Contracts.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InspireEd.Presentation.Controllers;

/// <summary>
/// API Controller for managing auth-related operations.
/// </summary>
[Route("api/auth")]
public sealed class AuthController(ISender sender) : ApiController(sender)
{
    /// <summary>
    /// Logs in a user by validating their credentials and generating a token.
    /// </summary>
    /// <param name="request">The login request containing the user's email and password.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An IActionResult containing the token if successful, or an error message.</returns>
    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);

        var tokenResult = await Sender.Send(command, cancellationToken);

        return tokenResult.IsFailure ? HandleFailure(tokenResult) : Ok(tokenResult.Value);
    }
}