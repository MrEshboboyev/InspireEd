using InspireEd.Application.Faculties.Commands.CreateFaculty;
using InspireEd.Application.Faculties.Commands.DeleteFaculty;
using InspireEd.Application.Faculties.Commands.DepartmentHeads.AddDepartmentHead;
using InspireEd.Application.Faculties.Commands.DepartmentHeads.CreateDepartmentHead;
using InspireEd.Application.Faculties.Commands.DepartmentHeads.DeleteDepartmentHeadCommand;
using InspireEd.Application.Faculties.Commands.DepartmentHeads.RemoveDepartmentHead;
using InspireEd.Application.Faculties.Commands.Groups.AddGroupToFaculty;
using InspireEd.Application.Faculties.Commands.Groups.RemoveGroupFromFaculty;
using InspireEd.Application.Faculties.Commands.Groups.UpdateGroup;
using InspireEd.Application.Faculties.Commands.UpdateFaculty;
using InspireEd.Application.Faculties.Queries.GetFaculties;
using InspireEd.Domain.Users.Enums;
using InspireEd.Infrastructure.Authentication;
using InspireEd.Presentation.Abstractions;
using InspireEd.Presentation.Contracts.Admins;
using InspireEd.Presentation.Contracts.Admins.DepartmentHeads;
using InspireEd.Presentation.Contracts.Admins.Faculties;
using InspireEd.Presentation.Contracts.Admins.Groups;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InspireEd.Presentation.Controllers;

[Route("api/admin")]
[HasPermission(Permission.FullAccess)]
public class AdminsController(ISender sender) : ApiController(sender)
{
    #region Department Head

    [HttpPost("create-department-head")]
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
    
    [HttpPost("delete-department-head")]
    public async Task<IActionResult> DeleteDepartmentHead(
        [FromBody] DeleteDepartmentHeadRequest request,
        CancellationToken cancellationToken)
    {
        var command = new DeleteDepartmentHeadCommand(
            request.DepartmentHeadId);
        
        var response = await Sender.Send(command, cancellationToken);
        
        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    [HttpPut("add-department-head-to-faculty")]
    public async Task<IActionResult> AddDepartmentHeadToFaculty(
        [FromBody] AddDepartmentHeadRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddDepartmentHeadCommand(
            request.FacultyId,
            request.DepartmentHeadId);
        
        var response = await Sender.Send(command, cancellationToken);
        
        return response.IsSuccess ? NoContent() : BadRequest(response);
    }
    
    [HttpPut("remove-department-head-from-faculty")]
    public async Task<IActionResult> RemoveDepartmentHeadFromFaculty(
        [FromBody] RemoveDepartmentHeadRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RemoveDepartmentHeadFromFacultyCommand(
            request.FacultyId,
            request.DepartmentHeadId);
        
        var response = await Sender.Send(command, cancellationToken);
        
        return response.IsSuccess ? NoContent() : BadRequest(response);
    }
    
    #endregion
    
    #region Faculties

    [HttpGet("faculties")]
    public async Task<IActionResult> GetAllFaculties(
        CancellationToken cancellationToken)
    {
        var query = new GetFacultiesQuery();
        
        var response = await Sender.Send(query, cancellationToken);
        
        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    [HttpPost("create-faculty")]
    public async Task<IActionResult> CreateFaculty(
        [FromBody] CreateFacultyRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateFacultyCommand(
            request.FacultyName);
        
        var response = await Sender.Send(command, cancellationToken);
        
        return response.IsSuccess ? NoContent() : BadRequest(response);
    }
    
    [HttpPost("update-faculty")]
    public async Task<IActionResult> UpdateFaculty(
        [FromBody] UpdateFacultyRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateFacultyCommand(
            request.FacultyId,
            request.FacultyName);
        
        var response = await Sender.Send(command, cancellationToken);
        
        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    [HttpPost("delete-faculty")]
    public async Task<IActionResult> DeleteFaculty(
        [FromBody] DeleteFacultyRequest request,
        CancellationToken cancellationToken)
    {
        var command = new DeleteFacultyCommand(
            request.FacultyId);
        
        var response = await Sender.Send(command, cancellationToken);
        
        return response.IsSuccess ? NoContent() : BadRequest(response);
    }
    
    #endregion
    
    #region Groups

    [HttpPost("faculties/{facultyId:guid}/groups")]
    public async Task<IActionResult> CreateGroup(
        Guid facultyId,
        [FromBody] AddGroupToFacultyRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddGroupToFacultyCommand(
            facultyId,
            request.GroupName);
        
        var response = await Sender.Send(command, cancellationToken);
        
        return response.IsSuccess ? NoContent() : BadRequest(response);
    }
    
    [HttpDelete("faculties/{facultyId:guid}/groups/{groupId:guid}")]
    public async Task<IActionResult> RemoveGroup(
        Guid facultyId,
        Guid groupId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveGroupFromFacultyCommand(
            facultyId,
            groupId);
        
        var response = await Sender.Send(command, cancellationToken);
        
        return response.IsSuccess ? NoContent() : BadRequest(response);
    }
    
    [HttpPut("faculties/{facultyId:guid}/groups/{groupId:guid}")]
    public async Task<IActionResult> UpdateGroup(
        Guid facultyId,
        Guid groupId,
        [FromBody] UpdateGroupRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateGroupCommand(
            facultyId,
            groupId,
            request.GroupName);
        
        var response = await Sender.Send(command, cancellationToken);
        
        return response.IsSuccess ? NoContent() : BadRequest(response);
    }
    
    #endregion
}