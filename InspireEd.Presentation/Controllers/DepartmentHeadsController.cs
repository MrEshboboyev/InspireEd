using InspireEd.Application.Faculties.Groups.Commands.AddMultipleStudentsToGroup;
using InspireEd.Application.Faculties.Groups.Commands.AddStudentToGroup;
using InspireEd.Application.Faculties.Groups.Commands.RemoveStudentFromGroup;
using InspireEd.Domain.Users.Enums;
using InspireEd.Infrastructure.Authentication;
using InspireEd.Presentation.Abstractions;
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
    
    
    
    #endregion
}