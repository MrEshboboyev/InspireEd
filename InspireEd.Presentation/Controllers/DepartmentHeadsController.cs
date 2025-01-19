using InspireEd.Application.Classes.Attendances.Commands.DeleteAttendance;
using InspireEd.Application.Classes.Attendances.Commands.UpdateAttendance;
using InspireEd.Application.Classes.Attendances.Queries.GetAttendanceById;
using InspireEd.Application.Classes.Commands.CreateClass;
using InspireEd.Application.Classes.Commands.DeleteClass;
using InspireEd.Application.Classes.Commands.RescheduleClass;
using InspireEd.Application.Classes.Commands.UpdateClass;
using InspireEd.Application.Classes.Commands.UpdateGroupIds;
using InspireEd.Application.Classes.Queries.GetAttendancesByClassId;
using InspireEd.Application.Classes.Queries.GetClassById;
using InspireEd.Application.Classes.Queries.GetClassesBySubjectId;
using InspireEd.Application.Faculties.Commands.MergeGroups;
using InspireEd.Application.Faculties.Commands.SplitGroup;
using InspireEd.Application.Faculties.Groups.Commands.AddMultipleStudentsToGroup;
using InspireEd.Application.Faculties.Groups.Commands.AddStudentToGroup;
using InspireEd.Application.Faculties.Groups.Commands.RemoveAllStudentsFromGroup;
using InspireEd.Application.Faculties.Groups.Commands.RemoveStudentFromGroup;
using InspireEd.Application.Faculties.Groups.Commands.TransferStudentBetweenGroups;
using InspireEd.Application.Faculties.Groups.Queries.GetStudentDetails;
using InspireEd.Application.Subjects.Commands.ChangeSubjectCredit;
using InspireEd.Application.Subjects.Commands.CreateSubject;
using InspireEd.Application.Subjects.Commands.DeleteSubject;
using InspireEd.Application.Subjects.Commands.RenameSubject;
using InspireEd.Application.Subjects.Commands.UpdateSubject;
using InspireEd.Application.Subjects.Queries.GetAllSubjects;
using InspireEd.Application.Subjects.Queries.GetSubjectById;
using InspireEd.Application.Subjects.Queries.GetSubjectsByCreationDateRange;
using InspireEd.Application.Subjects.Queries.GetSubjectsByCreditRange;
using InspireEd.Domain.Users.Enums;
using InspireEd.Infrastructure.Authentication;
using InspireEd.Presentation.Abstractions;
using InspireEd.Presentation.Contracts.DepartmentHeads.Classes;
using InspireEd.Presentation.Contracts.DepartmentHeads.Classes.Attendances;
using InspireEd.Presentation.Contracts.DepartmentHeads.Groups;
using InspireEd.Presentation.Contracts.DepartmentHeads.Subjects;
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

    #region Students

    #region Get

    /// <summary>
    /// Retrieves details of a student by their unique identifier.
    /// </summary>
    /// <param name="studentId">The unique identifier of the student.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet("{studentId:guid}")]
    public async Task<IActionResult> GetStudentDetails(
        Guid studentId,
        CancellationToken cancellationToken)
    {
        var query = new GetStudentDetailsQuery(studentId);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    #endregion

    #endregion

    #endregion

    #region Class related

    #region Get

    /// <summary>
    /// Retrieves details of a class by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the class.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HasPermission(Permission.ViewAssignedClasses)]
    [HttpGet("classes/{id:guid}")]
    public async Task<IActionResult> GetClassById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetClassByIdQuery(id);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    /// <summary>
    /// Retrieves classes by the subject's unique identifier.
    /// </summary>
    /// <param name="subjectId">The unique identifier of the subject.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet("classes/{subjectId:guid}")]
    public async Task<IActionResult> GetClassesBySubjectId(
        Guid subjectId,
        CancellationToken cancellationToken)
    {
        var query = new GetClassesBySubjectIdQuery(subjectId);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    /// <summary>
    /// Retrieves attendance records by the class's unique identifier.
    /// </summary>
    /// <param name="classId">The unique identifier of the class.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet("classes/{classId:guid}/attendances")]
    public async Task<IActionResult> GetAttendancesByClassId(
        Guid classId,
        CancellationToken cancellationToken)
    {
        var query = new GetAttendancesByClassIdQuery(classId);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }


    /// <summary>
    /// Retrieves an attendance record by its unique identifier.
    /// </summary>
    /// <param name="attendanceId">The unique identifier of the attendance record.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet("{attendanceId:guid}")]
    public async Task<IActionResult> GetAttendanceById(
        Guid attendanceId,
        CancellationToken cancellationToken)
    {
        var query = new GetAttendanceByIdQuery(attendanceId);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    #endregion

    [HasPermission(Permission.AssignClasses)]
    [HttpPost("classes")]
    public async Task<IActionResult> CreateClass(
        [FromBody] CreateClassRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateClassCommand(
            request.SubjectId,
            request.TeacherId,
            request.ClassType,
            request.GroupIds,
            request.ScheduledDate);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    /// <summary>
    /// Updates a class's details.
    /// </summary>
    /// <param name="classId">The unique identifier of the class.</param>
    /// <param name="request">The request containing updated class details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpPut("classes/{classId:guid}")]
    public async Task<IActionResult> UpdateClass(
        Guid classId,
        [FromBody] UpdateClassRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateClassCommand(
            classId,
            request.SubjectId,
            request.TeacherId,
            request.Type,
            request.ScheduledDate);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    /// <summary>
    /// Deletes a class by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the class.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpDelete("classes/{id:guid}")]
    public async Task<IActionResult> DeleteClass(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteClassCommand(id);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    /// <summary>
    /// Updates the group IDs associated with the class.
    /// </summary>
    /// <param name="classId">The unique identifier of the class.</param>
    /// <param name="request">The request containing the new group IDs.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpPut("classes/{classId:guid}/group-ids")]
    public async Task<IActionResult> UpdateGroupIds(
        Guid classId,
        [FromBody] UpdateGroupIdsRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateGroupIdsCommand(
            classId,
            request.GroupIds);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    /// <summary>
    /// Reschedules a class to a new date.
    /// </summary>
    /// <param name="id">The unique identifier of the class.</param>
    /// <param name="request">The request containing the new scheduled date.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpPut("{id:guid}/reschedule")]
    public async Task<IActionResult> RescheduleClass(
        Guid id,
        [FromBody] RescheduleClassRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RescheduleClassCommand(
            id,
            request.NewScheduledDate);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    #region Attendance related

    /// <summary>
    /// Updates an attendance record for a student in a class.
    /// </summary>
    /// <param name="classId">The unique identifier of the class.</param>
    /// <param name="attendanceId">The unique identifier of the attendance record.</param>
    /// <param name="request">The request containing updated attendance details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpPut("classes/{classId:guid}/attendances/{attendanceId:guid}")]
    public async Task<IActionResult> UpdateAttendance(
        Guid classId,
        Guid attendanceId,
        [FromBody] UpdateAttendanceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateAttendanceCommand(
            classId,
            attendanceId,
            request.AttendanceStatus,
            request.Notes);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    /// <summary>
    /// Deletes an attendance record by its unique identifier.
    /// </summary>
    /// <param name="classId">The unique identifier of the class.</param>
    /// <param name="attendanceId">The unique identifier of the attendance record.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpDelete("classes/{classId:guid}/attendances/{attendanceId:guid}")]
    public async Task<IActionResult> DeleteAttendance(
        Guid classId,
        Guid attendanceId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteAttendanceCommand(classId, attendanceId);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    #endregion

    #endregion

    #region Subject related

    #region Get

    /// <summary>
    /// Retrieves details of a subject by its unique identifier.
    /// </summary>
    /// <param name="subjectId">The unique identifier of the subject.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet("subjects/{subjectId:guid}")]
    public async Task<IActionResult> GetSubjectById(
        Guid subjectId,
        CancellationToken cancellationToken)
    {
        var query = new GetSubjectByIdQuery(subjectId);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    /// <summary>
    /// Retrieves all subjects.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet]
    public async Task<IActionResult> GetAllSubjects(
        CancellationToken cancellationToken)
    {
        var query = new GetAllSubjectsQuery();

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    /// <summary>
    /// Retrieves subjects within a specific credit range.
    /// </summary>
    /// <param name="minCredit">The minimum credit value.</param>
    /// <param name="maxCredit">The maximum credit value.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet("credit-range")]
    public async Task<IActionResult> GetSubjectsByCreditRange(
        int minCredit,
        int maxCredit,
        CancellationToken cancellationToken)
    {
        var query = new GetSubjectsByCreditRangeQuery(minCredit, maxCredit);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    /// <summary>
    /// Retrieves subjects created within a specified date range.
    /// </summary>
    /// <param name="startDate">The start date of the range.</param>
    /// <param name="endDate">The end date of the range.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet("subjects/creation-date-range")]
    public async Task<IActionResult> GetSubjectsByCreationDateRange(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken)
    {
        var query = new GetSubjectsByCreationDateRangeQuery(startDate, endDate);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    #endregion

    /// <summary>
    /// Creates a new subject.
    /// </summary>
    /// <param name="request">The request containing subject details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpPost("subjects")]
    public async Task<IActionResult> CreateSubject(
        [FromBody] CreateSubjectRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateSubjectCommand(
            request.Name,
            request.Code,
            request.Credit);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    /// <summary>
    /// Updates an existing subject's details.
    /// </summary>
    /// <param name="id">The unique identifier of the subject.</param>
    /// <param name="request">The request containing updated subject details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpPut("subjects/{id:guid}")]
    public async Task<IActionResult> UpdateSubject(
        Guid id,
        [FromBody] UpdateSubjectRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateSubjectCommand(
            id,
            request.Name,
            request.Code,
            request.Credit);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    /// <summary>
    /// Deletes a subject by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the subject.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpDelete("subjects/{id:guid}")]
    public async Task<IActionResult> DeleteSubject(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteSubjectCommand(id);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    /// <summary>
    /// Changes the credit value of a subject.
    /// </summary>
    /// <param name="id">The unique identifier of the subject.</param>
    /// <param name="request">The request containing the new credit value.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpPut("subjects/{id:guid}/change-credit")]
    public async Task<IActionResult> ChangeSubjectCredit(
        Guid id,
        [FromBody] ChangeSubjectCreditRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangeSubjectCreditCommand(
            id,
            request.NewCredit);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    /// <summary>
    /// Renames a subject.
    /// </summary>
    /// <param name="id">The unique identifier of the subject.</param>
    /// <param name="request">The request containing the new name for the subject.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpPut("subjects/{id:guid}/rename")]
    public async Task<IActionResult> RenameSubject(
        Guid id,
        [FromBody] RenameSubjectRequest request,
        CancellationToken cancellationToken)
    {
        var command = new RenameSubjectCommand(
            id,
            request.NewName);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    #endregion
}