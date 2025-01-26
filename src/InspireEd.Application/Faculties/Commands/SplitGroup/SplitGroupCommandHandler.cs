using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Faculties.Commands.SplitGroup;

internal sealed class SplitGroupCommandHandler(
    IFacultyRepository facultyRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<SplitGroupCommand>
{
    public async Task<Result> Handle(
        SplitGroupCommand request,
        CancellationToken cancellationToken)
    {
        var (facultyId, groupId, numberOfGroups) = request;

        #region Get Faculty and Group

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
                DomainErrors.Faculty.GroupDoesNotExist(groupId));
        }

        #endregion

        #region Split Group

        var splitResult = faculty.SplitGroup(group, numberOfGroups);
        if (splitResult.IsFailure)
        {
            return splitResult;
        }

        #endregion

        #region Update and Save Changes to Database

        facultyRepository.Update(faculty);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}