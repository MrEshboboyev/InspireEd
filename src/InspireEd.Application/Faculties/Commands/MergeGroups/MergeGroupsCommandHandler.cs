using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Faculties.Commands.MergeGroups;

internal sealed class MergeGroupsCommandHandler(
    IFacultyRepository facultyRepository,
    IGroupRepository groupRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<MergeGroupsCommand>
{
    public async Task<Result> Handle(
        MergeGroupsCommand request,
        CancellationToken cancellationToken)
    {
        var (facultyId, groupIds) = request;

        #region Get Faculty and Groups

        var faculty = await facultyRepository.GetByIdAsync(facultyId, cancellationToken);
        if (faculty is null)
        {
            return Result.Failure(DomainErrors.Faculty.NotFound(facultyId));
        }

        var groups = faculty.GetGroupsByIds(groupIds);
        if (groups.Count != groupIds.Count)
        {
            return Result.Failure(
                DomainErrors.Faculty.SomeGroupsNotFound);
        }

        #endregion

        #region Merge Groups

        var mergedGroup = faculty.MergeGroups(groups);
        if (mergedGroup.IsFailure)
        {
            return mergedGroup;
        }

        foreach (var group in groups)
        {
            faculty.RemoveGroup(group.Id);
            
            #region Remove from db
            
            groupRepository.Remove(group);
            
            #endregion
        }

        #endregion

        #region Save Changes to Database

        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}