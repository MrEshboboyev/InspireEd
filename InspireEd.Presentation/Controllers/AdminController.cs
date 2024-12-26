using InspireEd.Application.Faculties.Commands.AddDepartmentHead;
using InspireEd.Presentation.Abstractions;
using InspireEd.Presentation.Contracts.Admins;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InspireEd.Presentation.Controllers;

[Route("api/admin")]
public class AdminController(ISender sender) : ApiController(sender)
{
    #region Department Head

    [HttpGet("add-department-head")]
    public async Task<IActionResult> AddDepartmentHead(
        [FromBody] AddDepartmentHeadRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddDepartmentHeadCommand(
            request.FacultyId,
            request.DepartmentHeadId);
        
        var response = await Sender.Send(command, cancellationToken);
        
        return response.IsSuccess ? NoContent() : BadRequest(response);
    }
    
    #endregion
}