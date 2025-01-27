using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Repositories;

namespace InspireEd.Application.Faculties.Groups.Commands.RemoveAllStudentsFromGroup;

internal sealed class RemoveAllStudentsFromGroupCommandHandler(
    IFacultyRepository facultyRepository,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RemoveAllStudentsFromGroupCommand>
{
    public async Task<Result> Handle(
        RemoveAllStudentsFromGroupCommand request,
        CancellationToken cancellationToken)
    {
        var (facultyId, groupId) = request;

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

        #region Remove all students from group

        var studentIds = group.StudentIds.ToList();
        var users = await userRepository.GetByIdsAsync(studentIds, cancellationToken);

        foreach (var studentId in studentIds)
        {
            var removeStudentFromGroupResult = group.RemoveStudent(studentId);
            if (removeStudentFromGroupResult.IsFailure)
            {
                return Result.Failure(removeStudentFromGroupResult.Error);
            }

            var user = users.SingleOrDefault(u => u.Id == studentId);
            if (user is null)
            {
                return Result.Failure(
                    DomainErrors.User.NotFound(studentId));
            }

            userRepository.Delete(user);
        }

        #endregion

        #region Update and save changes

        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}