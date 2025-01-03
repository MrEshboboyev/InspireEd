using InspireEd.Application.Faculties.Commands.MergeGroups;
using InspireEd.Application.Faculties.Commands.SplitGroup;
using InspireEd.Application.Faculties.Groups.Commands.AddMultipleStudentsToGroup;
using InspireEd.Application.Faculties.Groups.Commands.AddStudentToGroup;
using InspireEd.Application.Faculties.Groups.Commands.RemoveAllStudentsFromGroup;
using InspireEd.Application.Faculties.Groups.Commands.RemoveStudentFromGroup;
using InspireEd.Application.Faculties.Groups.Commands.TransferStudentBetweenGroups;
using InspireEd.Domain.Users.Enums;
using InspireEd.Infrastructure.Authentication;
using InspireEd.Presentation.Abstractions;
using InspireEd.Presentation.Contracts.DepartmentHeads.Groups;
using InspireEd.Presentation.Contracts.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InspireEd.Presentation.Controllers;

[Route("api/department-heads")]
public class DepartmentHeadsController(ISender sender) : ApiController(sender)
{
    #region Group related

    [HasPermission(Permission.AddStudents)]
    [HttpPost("faculties/{facultyId:guid}/groups/{groupId:guid}/add-multiple-students")]
    public async Task<IActionResult> AddMultipleStudentsToGroup(
        Guid facultyId,
        Guid groupId,
        [FromBody] AddMultipleUsersRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddMultipleStudentsToGroupCommand(
            facultyId,
            groupId,
            request.Users.Select(student =>
                    (student.FirstName, student.LastName, student.Email, student.Password))
                .ToList());

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }


    [HasPermission(Permission.AddStudents)]
    [HttpPost("faculties/{facultyId:guid}/groups/{groupId:guid}/students")]
    public async Task<IActionResult> AddStudentToGroup(
        Guid facultyId,
        Guid groupId,
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new AddStudentToGroupCommand(
            facultyId,
            groupId,
            request.FirstName,
            request.LastName,
            request.Email,
            request.Password);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    [HasPermission(Permission.AddStudents)]
    [HttpDelete("faculties/{facultyId:guid}/groups/" +
                "{groupId:guid}/remove-all-students")]
    public async Task<IActionResult> RemoveAllStudentsFromGroup(
        Guid facultyId,
        Guid groupId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveAllStudentsFromGroupCommand(
            facultyId,
            groupId);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    [HasPermission(Permission.AddStudents)]
    [HttpDelete("faculties/{facultyId:guid}/groups/" +
                "{groupId:guid}/students/{studentId:guid}")]
    public async Task<IActionResult> RemoveStudentFromGroup(
        Guid facultyId,
        Guid groupId,
        Guid studentId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveStudentFromGroupCommand(
            facultyId,
            groupId,
            studentId);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    [HasPermission(Permission.AddStudents)]
    [HttpPut("faculties/{facultyId:guid}/groups/{sourceGroupId:guid}" +
             "/students/{studentId:guid}/transfer/{targetGroupId:guid}")]
    public async Task<IActionResult> TransferStudentBetweenGroups(
        Guid facultyId,
        Guid sourceGroupId,
        Guid targetGroupId,
        Guid studentId,
        CancellationToken cancellationToken)
    {
        var command = new TransferStudentBetweenGroupsCommand(
            facultyId,
            sourceGroupId,
            targetGroupId,
            studentId);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    [HasPermission(Permission.ManageGroups)]
    [HttpPost("faculties/{facultyId:guid}/groups/merge")]
    public async Task<IActionResult> MergeGroups(
        Guid facultyId,
        [FromBody] MergeGroupsRequest request,
        CancellationToken cancellationToken)
    {
        var command = new MergeGroupsCommand(
            facultyId,
            request.GroupIds);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    [HasPermission(Permission.ManageGroups)]
    [HttpPost("faculties/{facultyId:guid}/groups/{groupId:guid}/split")]
    public async Task<IActionResult> SplitGroup(
        Guid facultyId,
        Guid groupId,
        [FromBody] SplitGroupRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SplitGroupCommand(
            facultyId,
            groupId,
            request.NumberOfGroups);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    #endregion
}