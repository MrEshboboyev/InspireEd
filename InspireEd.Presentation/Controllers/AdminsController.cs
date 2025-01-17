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
using InspireEd.Application.Faculties.Groups.Queries.GetAllStudentsInFaculty;
using InspireEd.Application.Faculties.Groups.Queries.GetGroupDetails;
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

[Route("api/admins")]
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

    [HttpPost("department-heads")]
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
        Guid departmentHeadId,
        [FromBody] UpdateDepartmentHeadDetailsRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateDepartmentHeadDetailsCommand(
            departmentHeadId,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    [HttpDelete("department-heads/{departmentHeadId:guid}")]
    public async Task<IActionResult> DeleteDepartmentHead(
        Guid departmentHeadId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteDepartmentHeadCommand(departmentHeadId);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    [HttpPut("faculties/{facultyId:guid}/add-department-head")]
    public async Task<IActionResult> AddDepartmentHeadToFaculty(
        Guid facultyId,
        Guid departmentHeadId,
        CancellationToken cancellationToken)
    {
        var command = new AddDepartmentHeadToFacultyCommand(
            facultyId,
            departmentHeadId);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    [HttpPut("faculties/{facultyId:guid}/remove-department-head")]
    public async Task<IActionResult> RemoveDepartmentHeadFromFaculty(
        Guid facultyId,
        Guid departmentHeadId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveDepartmentHeadFromFacultyCommand(
            facultyId,
            departmentHeadId);

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
    [HttpGet("faculties/{facultyId:guid}")]
    public async Task<IActionResult> GetFacultyDetails(
        Guid facultyId,
        CancellationToken cancellationToken)
    {
        var query = new GetFacultyDetailsQuery(facultyId);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    /// <summary>
    /// Retrieves all students in a faculty by its unique identifier.
    /// </summary>
    /// <param name="facultyId">The unique identifier of the faculty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet("faculties/{facultyId:guid}/students")]
    public async Task<IActionResult> GetAllStudentsInFaculty(
        Guid facultyId,
        CancellationToken cancellationToken)
    {
        var query = new GetAllStudentsInFacultyQuery(facultyId);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    #endregion

    [HttpPost("faculties")]
    public async Task<IActionResult> CreateFaculty(
        [FromBody] CreateFacultyRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateFacultyCommand(
            request.FacultyName);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    [HttpPut("faculties/{facultyId:guid}")]
    public async Task<IActionResult> UpdateFaculty(
        Guid facultyId,
        [FromBody] UpdateFacultyRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateFacultyCommand(
            facultyId,
            request.FacultyName);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    [HttpDelete("faculties/{facultyId:guid}")]
    public async Task<IActionResult> DeleteFaculty(
        Guid facultyId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteFacultyCommand(facultyId);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    #endregion

    #region Groups

    #region Get

    /// <summary>
    /// Retrieves details of a group by its unique identifier.
    /// </summary>
    /// <param name="groupId">The unique identifier of the group.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet("groups/{groupId:guid}")]
    public async Task<IActionResult> GetGroupDetails(
        Guid groupId,
        CancellationToken cancellationToken)
    {
        var query = new GetGroupDetailsQuery(groupId);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    #endregion

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