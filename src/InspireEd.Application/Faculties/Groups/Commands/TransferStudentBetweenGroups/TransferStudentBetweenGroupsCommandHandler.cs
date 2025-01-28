using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Faculties.Groups.Commands.TransferStudentBetweenGroups;

internal sealed class TransferStudentBetweenGroupsCommandHandler(
    IFacultyRepository facultyRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<TransferStudentBetweenGroupsCommand>
{
    public async Task<Result> Handle(
        TransferStudentBetweenGroupsCommand request,
        CancellationToken cancellationToken)
    {
        var (facultyId, sourceGroupId, targetGroupId, studentId) = request;

        #region Get Faculty and Groups

        var faculty = await facultyRepository.GetByIdWithGroupsAsync(
            facultyId,
            cancellationToken);
        if (faculty is null)
        {
            return Result.Failure(
                DomainErrors.Faculty.NotFound(facultyId));
        }

        var sourceGroup = faculty.GetGroupById(sourceGroupId);
        if (sourceGroup is null)
        {
            return Result.Failure(
                DomainErrors.Faculty.GroupDoesNotExist(sourceGroupId));
        }

        var targetGroup = faculty.GetGroupById(targetGroupId);
        if (targetGroup is null)
        {
            return Result.Failure(
                DomainErrors.Faculty.GroupDoesNotExist(targetGroupId));
        }

        #endregion

        #region Remove Student from Source Group

        var removeStudentResult = sourceGroup.RemoveStudent(studentId);
        if (removeStudentResult.IsFailure)
        {
            return Result.Failure(
                removeStudentResult.Error);
        }

        #endregion

        #region Add Student to Target Group

        var addStudentResult = targetGroup.AddStudent(studentId);
        if (addStudentResult.IsFailure)
        {
            // Rollback removal from source group
            sourceGroup.AddStudent(studentId);

            return Result.Failure(
                addStudentResult.Error);
        }

        #endregion

        #region Save Changes to Database

        facultyRepository.Update(faculty);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion

        return Result.Success();
    }
}