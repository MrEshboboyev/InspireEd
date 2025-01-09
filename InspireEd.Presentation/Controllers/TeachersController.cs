using InspireEd.Application.Classes.Commands.CreateAttendances;
using InspireEd.Domain.Users.Enums;
using InspireEd.Infrastructure.Authentication;
using InspireEd.Presentation.Abstractions;
using InspireEd.Presentation.Contracts.Teachers.Classes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InspireEd.Presentation.Controllers;

[Route("api/teacher")]
public class TeachersController(ISender sender) : ApiController(sender)
{
    /// <summary>
    /// Creates attendance records for students in a class.
    /// </summary>
    /// <param name="classId">The unique identifier of the class.</param>
    /// <param name="request">The request containing attendance details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HasPermission(Permission.ManageAttendance)]
    [HttpPost("classes/{classId:guid}/attendances")]
    public async Task<IActionResult> CreateAttendances(
        Guid classId,
        [FromBody] CreateAttendancesRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateAttendancesCommand(
            classId,
            request.Attendances);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }
}