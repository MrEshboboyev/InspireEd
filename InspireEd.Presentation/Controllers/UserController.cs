using InspireEd.Application.Users.Commands.CreateUser;
using InspireEd.Application.Users.Commands.Login;
using InspireEd.Application.Users.Queries.GetUserById;
using InspireEd.Domain.Shared;
using InspireEd.Presentation.Abstractions;
using InspireEd.Presentation.Contracts.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InspireEd.Presentation.Controllers;

[Route("api/users")]
public sealed class UsersController(ISender sender) : ApiController(sender)
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);
        Result<UserResponse> response = await Sender.Send(query, cancellationToken);
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser(
            [FromBody] LoginRequest request,
            CancellationToken cancellationToken)
    {
        var command = new LoginCommand(request.Email, request.Password);

        Result<string> tokenResult = await Sender.Send(
            command,
            cancellationToken);

        if (tokenResult.IsFailure)
        {
            return HandleFailure(tokenResult);
        }

        return Ok(tokenResult.Value);
    }

    [HttpPost]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName);

        Result<Guid> result = await Sender.Send(command, cancellationToken);
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