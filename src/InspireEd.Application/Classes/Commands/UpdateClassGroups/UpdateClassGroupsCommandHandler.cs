using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.Repositories;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Classes.Commands.UpdateClassGroups;

internal sealed class UpdateClassGroupsCommandHandler(
    IClassRepository classRepository,
    IGroupRepository groupRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateClassGroupsCommand>
{
    public async Task<Result> Handle(
        UpdateClassGroupsCommand request,
        CancellationToken cancellationToken)
    {
        var (classId, groupIds) = request;
        
        #region Get this Class and Groups
        
        var classEntity = await classRepository.GetByIdAsync(
            classId,
            cancellationToken);
        if (classEntity is null)
        {
            return Result.Failure(
                DomainErrors.Class.NotFound(classId));
        }
        
        // Get Groups
        var groups = await groupRepository.GetByIdsAsync(
            groupIds,
            cancellationToken);

        // Ensure all requested groups exist
        var missingGroups = groupIds
            .Except(groups.Select(g => g.Id))
            .ToList();
        if (missingGroups.Count != 0)
        {
            return Result.Failure(
                DomainErrors.Group.MissingGroups(missingGroups));
        }
        
        #endregion
        
        #region Update group ids

        var updateGroupIdsResult = classEntity.UpdateGroups(groupIds);
        if (updateGroupIdsResult.IsFailure)
        {
            return Result.Failure(
                updateGroupIdsResult.Error);
        }
        
        #endregion
        
        #region Update database
        
        classRepository.Update(classEntity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}