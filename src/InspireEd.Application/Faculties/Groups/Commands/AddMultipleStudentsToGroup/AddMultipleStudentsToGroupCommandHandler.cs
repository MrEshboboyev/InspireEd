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

        var faculty = await facultyRepository.GetByIdAsync(facultyId, cancellationToken);
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
            var createUserResult = await userCreationService.CreateUserAsync(
                firstName,
                lastName,
                email,
                password,
                Role.Student.ToString(),
                cancellationToken);

            if (createUserResult.IsFailure)
            {
                return Result.Failure(createUserResult.Error);
            }

            var userId = createUserResult.Value;

            var addStudentResult = group.AddStudent(userId);
            if (addStudentResult.IsFailure)
            {
                return addStudentResult;
            }
        }

        #endregion

        #region Save changes to database

        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}