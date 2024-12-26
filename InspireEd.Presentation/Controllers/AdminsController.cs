using InspireEd.Application.Faculties.Commands.AddDepartmentHead;
using InspireEd.Application.Faculties.Commands.CreateDepartmentHead;
using InspireEd.Application.Faculties.Commands.RemoveDepartmentHeadCommand;
using InspireEd.Domain.Users.Enums;
using InspireEd.Infrastructure.Authentication;
using InspireEd.Presentation.Abstractions;
using InspireEd.Presentation.Contracts.Admins;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InspireEd.Presentation.Controllers;

[Route("api/admin")]
[HasPermission(Permission.FullAccess)]
public class AdminsController(ISender sender) : ApiController(sender)
{
    #region Department Head

    public async Task<IActionResult> CreateDepartmentHead(
        [FromBody] CreateDepartmentHeadRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateDepartmentHeadCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password);
        
        var response = await Sender.Send(command, cancellationToken);
        
        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

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
    
    [HttpGet("remove-department-head")]
    public async Task<IActionResult> AddDepartmentHead(
        [FromBody] RemoveDepartmentHeadRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RemoveDepartmentHeadCommand(
            request.FacultyId,
            request.DepartmentHeadId);
        
        var response = await Sender.Send(command, cancellationToken);
        
        return response.IsSuccess ? NoContent() : BadRequest(response);
    }
    
    #endregion
}