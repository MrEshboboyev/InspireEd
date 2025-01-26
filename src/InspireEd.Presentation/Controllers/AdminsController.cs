using InspireEd.Application.Faculties.Commands.AddDepartmentHeadToFaculty;
using InspireEd.Application.Faculties.Commands.AddGroupToFaculty;
using InspireEd.Application.Faculties.Commands.CreateFaculty;
using InspireEd.Application.Faculties.Commands.DeleteFaculty;
using InspireEd.Application.Faculties.Commands.RemoveDepartmentHeadFromFaculty;
using InspireEd.Application.Faculties.Commands.RemoveGroupFromFaculty;
using InspireEd.Application.Faculties.Commands.RenameFaculty;
using InspireEd.Application.Faculties.DepartmentHeads.Commands.CreateDepartmentHead;
using InspireEd.Application.Faculties.DepartmentHeads.Commands.DeleteDepartmentHead;
using InspireEd.Application.Faculties.DepartmentHeads.Queries.GetDepartmentHeadDetails;
using InspireEd.Application.Faculties.Groups.Commands.UpdateGroup;
using InspireEd.Application.Faculties.Groups.Queries.GetAllGroupsInFaculty;
using InspireEd.Application.Faculties.Groups.Queries.GetAllStudentsInFaculty;
using InspireEd.Application.Faculties.Groups.Queries.GetGroupDetails;
using InspireEd.Application.Faculties.Queries.GetAllDepartmentHeadsInFaculty;
using InspireEd.Application.Faculties.Queries.GetFaculties;
using InspireEd.Application.Faculties.Queries.GetFacultyDetails;
using InspireEd.Application.Users.Commands.AssignRoleToUser;
using InspireEd.Application.Users.Commands.ChangeUserPassword;
using InspireEd.Application.Users.Commands.CreateUser;
using InspireEd.Application.Users.Commands.DeleteUser;
using InspireEd.Application.Users.Commands.RemoveRoleFromUser;
using InspireEd.Application.Users.Commands.UpdateUser;
using InspireEd.Application.Users.Queries.GetAllUsers;
using InspireEd.Application.Users.Queries.GetUserByEmail;
using InspireEd.Application.Users.Queries.GetUserById;
using InspireEd.Application.Users.Queries.GetUserRoles;
using InspireEd.Application.Users.Queries.GetUsersByRole;
using InspireEd.Application.Users.Queries.SearchUsersByName;
using InspireEd.Domain.Users.Enums;
using InspireEd.Infrastructure.Authentication;
using InspireEd.Presentation.Abstractions;
using InspireEd.Presentation.Contracts.Admins.DepartmentHeads;
using InspireEd.Presentation.Contracts.Admins.Faculties;
using InspireEd.Presentation.Contracts.Admins.Groups;
using InspireEd.Presentation.Contracts.Admins.Users;
using InspireEd.Presentation.Contracts.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InspireEd.Presentation.Controllers;

[Route("api/admins")]
// [HasPermission(Permission.FullAccess)] // Manually hack, Until Admin is seeded into the database
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

    #region Create/Update/Delete
    
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

    [HttpDelete("department-heads/{departmentHeadId:guid}")]
    public async Task<IActionResult> DeleteDepartmentHead(
        Guid departmentHeadId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteDepartmentHeadCommand(departmentHeadId);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }

    #endregion
    
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

    /// <summary>
    /// Retrieves all groups within a faculty by its unique identifier.
    /// </summary>
    /// <param name="facultyId">The unique identifier of the faculty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet("faculties/{facultyId:guid}/groups")]
    public async Task<IActionResult> GetAllGroupsInFaculty(
        Guid facultyId,
        CancellationToken cancellationToken)
    {
        var query = new GetAllGroupsInFacultyQuery(facultyId);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    /// <summary>
    /// Retrieves all department heads within a faculty by its unique identifier.
    /// </summary>
    /// <param name="facultyId">The unique identifier of the faculty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet("faculties/{facultyId:guid}/department-heads")]
    public async Task<IActionResult> GetAllDepartmentHeadsInFaculty(
        Guid facultyId,
        CancellationToken cancellationToken)
    {
        var query = new GetAllDepartmentHeadsInFacultyQuery(facultyId);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    #endregion

    #region Create/Update/Delete
    
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

    [HttpDelete("faculties/{facultyId:guid}")]
    public async Task<IActionResult> DeleteFaculty(
        Guid facultyId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteFacultyCommand(facultyId);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }
    
    [HttpPut("faculties/{facultyId:guid}/rename/{newName}")]
    public async Task<IActionResult> RenameFaculty(
        Guid facultyId,
        string newName,
        CancellationToken cancellationToken)
    {
        var command = new RenameFacultyCommand(
            facultyId,
            newName);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response);
    }
    
    #endregion
    
    #region Department Head related
    
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

    #region Create/Update/Delete
    
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

    #endregion
    
    #endregion

    #region Users

    #region Get

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers(
        CancellationToken cancellationToken)
    {
        var query = new GetAllUsersQuery();

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    /// <summary>
    /// Retrieves the details of a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An IActionResult containing the user details if found, or an error message.</returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    /// <summary>
    /// Retrieves users by their role.
    /// </summary>
    /// <param name="roleId">The role identifier.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet("users/roleId")]
    public async Task<IActionResult> GetUsersByRole(
        [FromQuery] int roleId,
        CancellationToken cancellationToken)
    {
        var query = new GetUsersByRoleQuery(roleId);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet("users/email")]
    public async Task<IActionResult> GetUserByEmail(
        [FromQuery] string email,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByEmailQuery(email);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    [HttpGet("users/search")]
    public async Task<IActionResult> SearchUsersByName(
        [FromQuery] string searchTerm,
        CancellationToken cancellationToken)
    {
        var query = new SearchUsersByNameQuery(searchTerm);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    /// <summary>
    /// Retrieves the roles assigned to a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpGet("users/{userId:guid}/roles")]
    public async Task<IActionResult> GetUserRoles(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var query = new GetUserRolesQuery(userId);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }

    #endregion

    #region Create/Update/Delete
    
    /// <summary>
    /// Create a new user by creating their account with the provided details.
    /// </summary>
    /// <param name="request">The create request containing the user's details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>An IActionResult containing the new user's ID if successful, or an error message.</returns>
    [HttpPost("users")]
    public async Task<IActionResult> CreateUser(
        [FromBody] CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateUserCommand(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            request.RoleName);

        var result = await Sender.Send(command, cancellationToken);
        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(
            nameof(GetUserById),
            new { id = result.Value },
            result.Value);
    }
    
    /// <summary>
    /// Updates the details of a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="request">The request containing updated user details.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpPut("users/{userId:guid}")]
    public async Task<IActionResult> UpdateUser(
        Guid userId,
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserCommand(
            userId,
            request.FirstName,
            request.LastName);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response.Error);
    }
    
    /// <summary>
    /// Changes the password of a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="request">The request containing old and new passwords.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpPut("users/{userId:guid}/update-password")]
    public async Task<IActionResult> UpdateUserPassword(
        Guid userId,
        [FromBody] UpdateUserPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateUserPasswordCommand(
            userId,
            request.NewPassword);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response.Error);
    }

    /// <summary>
    /// Deletes a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpDelete("users/{userId:guid}")]
    public async Task<IActionResult> DeleteUser(
        Guid userId,
        CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand(userId);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? NoContent() : BadRequest(response.Error);
    }
    
    #endregion
    
    #region Role related

    /// <summary>
    /// Assigns a role to a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="roleId">The unique identifier the role.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpPut("users/{userId:guid}/assign-role/{roleId:int}")]
    public async Task<IActionResult> AssignRoleToUser(
        Guid userId,
        int roleId,
        CancellationToken cancellationToken)
    {
        var command = new AssignRoleToUserCommand(
            userId,
            roleId);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? Ok() : BadRequest(response.Error);
    }

    /// <summary>
    /// Removes a role from a user by their unique identifier.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="roleId">The unique identifier the role.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the action result.</returns>
    [HttpPut("users/{userId:guid}/remove-role/{roleId:int}")]
    public async Task<IActionResult> RemoveRoleFromUser(
        Guid userId,
        int roleId,
        CancellationToken cancellationToken)
    {
        var command = new RemoveRoleFromUserCommand(
            userId,
            roleId);

        var response = await Sender.Send(command, cancellationToken);

        return response.IsSuccess ? Ok() : BadRequest(response.Error);
    }
    
    #endregion

    #endregion
}