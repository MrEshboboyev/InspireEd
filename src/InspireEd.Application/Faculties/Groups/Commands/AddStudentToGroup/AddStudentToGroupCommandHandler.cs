using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Users.Services;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Entities;

namespace InspireEd.Application.Faculties.Groups.Commands.AddStudentToGroup;

internal sealed class AddStudentToGroupCommandHandler(
    IUserCreationService userCreationService,
    IFacultyRepository facultyRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddStudentToGroupCommand>
{
    public async Task<Result> Handle(
        AddStudentToGroupCommand request, 
        CancellationToken cancellationToken)
    {
        var (facultyId, groupId, studentFirstName, 
            studentLastName, studentEmail, studentPassword) = request;
        
        #region Get this faculty and group
        
        var faculty = await facultyRepository.GetByIdWithGroupsAsync(
            facultyId,
            cancellationToken);
        if (faculty is null)
        {
            return Result.Failure(
                DomainErrors.Faculty.NotFound(facultyId));
        }

        var group = faculty.GetGroupById(groupId);
        if (group is null)
        {
            return Result.Failure(
                DomainErrors.Faculty.GroupDoesNotExist(groupId));
        }

        #endregion
        
        #region Create Student

        var createStudentResult = await userCreationService.CreateUserAsync(
            studentFirstName,
            studentLastName,
            studentEmail,
            studentPassword,
            Role.Student.Name,
            cancellationToken);
        if (createStudentResult.IsFailure)
        {
            return Result.Failure(
                createStudentResult.Error);
        }

        #endregion
        
        #region Add Student Id to group
        
        var addStudentIdToGroupResult = group.AddStudent(createStudentResult.Value);
        if (addStudentIdToGroupResult.IsFailure)
        {
            return Result.Failure(
                addStudentIdToGroupResult.Error);
        }
        
        #endregion
        
        #region Update database
        
        facultyRepository.Update(faculty);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}