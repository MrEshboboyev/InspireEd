﻿using InspireEd.Application.Users.Commands.CreateUser;
using InspireEd.Application.Users.Commands.Login;
using InspireEd.Application.Users.Queries.GetUserById;
using InspireEd.Presentation.Abstractions;
using InspireEd.Presentation.Contracts.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InspireEd.Presentation.Controllers;

/// <summary>
/// API Controller for managing user-related operations.
/// </summary>
[Route("api/users")]
public sealed class UsersController(ISender sender) : ApiController(sender)
{
    /// <summary>
    /// Retrieves the details of a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An IActionResult containing the user details if found, or an error message.</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(
        Guid id, 
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

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

    /// <summary>
    /// Create a new user by creating their account with the provided details.
    /// </summary>
    /// <param name="request">The create request containing the user's details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An IActionResult containing the new user's ID if successful, or an error message.</returns>
    [HttpPost("create")]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName, 
            request.RoleName);

        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(
            nameof(GetUserById),
            new { id = result.Value },
            result.Value);
    }
}