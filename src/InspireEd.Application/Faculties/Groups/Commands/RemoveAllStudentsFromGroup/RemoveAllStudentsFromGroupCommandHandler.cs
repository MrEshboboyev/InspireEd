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

        // Start a unit of work transaction to ensure atomicity
        using var transaction = unitOfWork.BeginTransaction();

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

        #region Remove all students from group

        var studentIds = group.StudentIds.ToList();
        var users = await userRepository.GetByIdsAsync(studentIds, cancellationToken);

        foreach (var studentId in studentIds)
        {
            var removeStudentFromGroupResult = group.RemoveStudent(studentId);
            if (removeStudentFromGroupResult.IsFailure)
            {
                transaction.Rollback();
                return Result.Failure(removeStudentFromGroupResult.Error);
            }

            var user = users.SingleOrDefault(u => u.Id == studentId);
            if (user is null)
            {
                transaction.Rollback();
                return Result.Failure(
                    DomainErrors.User.NotFound(studentId));
            }

            userRepository.Delete(user);
        }

        #endregion

        #region Update and save changes

        await unitOfWork.SaveChangesAsync(cancellationToken);

        transaction.Commit();

        #endregion

        return Result.Success();
    }
}