using InspireEd.Application.Users.Queries.GetUserById;
using InspireEd.Domain.Users.Enums;
using InspireEd.Infrastructure.Authentication;
using InspireEd.Presentation.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InspireEd.Presentation.Controllers;

[Route("api/student")]
public sealed class StudentsController(ISender sender) : ApiController(sender)
{
    #region Get

    [HasPermission(Permission.ViewProfile)]
    [HttpGet("info")]
    public async Task<IActionResult> GetInfo(
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(GetCurrentUserId());
        
        var response = await Sender.Send(query, cancellationToken);
        
        return response.IsSuccess ? Ok(response.Value) : BadRequest(response.Error);
    }
    
    #endregion
}