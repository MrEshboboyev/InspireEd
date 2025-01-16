using InspireEd.Application.Faculties.Commands.AddDepartmentHeadToFaculty;
using InspireEd.Application.Faculties.Commands.AddGroupToFaculty;
using InspireEd.Application.Faculties.Commands.CreateFaculty;
using InspireEd.Application.Faculties.Commands.DeleteFaculty;
using InspireEd.Application.Faculties.Commands.RemoveDepartmentHeadFromFaculty;
using InspireEd.Application.Faculties.Commands.RemoveGroupFromFaculty;
using InspireEd.Application.Faculties.Commands.UpdateFaculty;
using InspireEd.Application.Faculties.DepartmentHeads.Commands.CreateDepartmentHead;
using InspireEd.Application.Faculties.DepartmentHeads.Commands.DeleteDepartmentHead;
using InspireEd.Application.Faculties.DepartmentHeads.Commands.UpdateDepartmentHeadDetails;
using InspireEd.Application.Faculties.DepartmentHeads.Queries.GetDepartmentHeadDetails;
using InspireEd.Application.Faculties.Groups.Commands.UpdateGroup;
using InspireEd.Application.Faculties.Queries.GetFaculties;
using InspireEd.Application.Faculties.Queries.GetFacultyDetails;
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

    #region Get

    /// <summary>
    /// Retrieves details of a department head by their unique identifier.
    /// </summary>
    /// <param name="departmentHeadId">The unique identifier of the department head.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet("department-heads/{departmentHeadId:guid}")]
    public async Task<IActionResult> GetDepartmentHeadDetails(
        Guid departmentHeadId,
        CancellationToken cancellationToken)
    {
        var query = new GetDepartmentHeadDetailsQuery(departmentHeadId);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    #endregion

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

    [HttpPut("department-heads/{departmentHeadId:guid}")]
    public async Task<IActionResult> UpdateDepartmentHeadDetails(
        Guid facultyId,
        Guid departmentHeadId,
        [FromBody] UpdateDepartmentHeadDetailsRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateDepartmentHeadDetailsCommand(
            facultyId,
            departmentHeadId,
            request.FirstName,
            request.LastName,
            request.Email);

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
        var command = new AddDepartmentHeadToFacultyCommand(
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

    #region Get

    [HttpGet("faculties")]
    public async Task<IActionResult> GetAllFaculties(
        CancellationToken cancellationToken)
    {
        var query = new GetFacultiesQuery();

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    /// <summary>
    /// Retrieves details of a faculty by its unique identifier.
    /// </summary>
    /// <param name="facultyId">The unique identifier of the faculty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet("{facultyId:guid}")]
    public async Task<IActionResult> GetFacultyDetails(
        Guid facultyId,
        CancellationToken cancellationToken)
    {
        var query = new GetFacultyDetailsQuery(facultyId);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    #endregion

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