using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Users.Services;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Entities;

namespace InspireEd.Application.Faculties.Groups.Commands.AddMultipleStudentsToGroup;

internal sealed class AddMultipleStudentsToGroupCommandHandler(
    IFacultyRepository facultyRepository,
    IUserCreationService userCreationService,
    IUnitOfWork unitOfWork) : ICommandHandler<AddMultipleStudentsToGroupCommand>
{
    public async Task<Result> Handle(
        AddMultipleStudentsToGroupCommand request,
        CancellationToken cancellationToken)
    {
        var (facultyId, groupId, students) = request;

        #region Get this faculty and group

        var faculty = await facultyRepository.GetByIdWithGroupsAsync(facultyId, cancellationToken);
        if (faculty is null)
        {
            return Result.Failure(
                DomainErrors.Faculty.NotFound(facultyId));
        }

        var group = faculty.GetGroupById(groupId);
        if (group is null)
        {
            return Result.Failure(
                DomainErrors.Faculty.GroupDoesNotExist(facultyId));
        }

        #endregion

        #region Add students to group

        foreach (var (firstName, lastName, email, password) in students)
        {
            #region Create new User with Student role
            
            var createUserResult = await userCreationService.CreateUserAsync(
                firstName,
                lastName,
                email,
                password,
                Role.Student.Name,
                cancellationToken);
            if (createUserResult.IsFailure)
            {
                return Result.Failure(createUserResult.Error);
            }
            
            #endregion

            #region Add this Student to group
            
            var addStudentResult = group.AddStudent(createUserResult.Value);
            if (addStudentResult.IsFailure)
            {
                return addStudentResult;
            }
            
            #endregion
        }

        #endregion

        #region Save changes to database

        facultyRepository.Update(faculty);  
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}