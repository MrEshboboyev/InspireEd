using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Classes.Commands.UpdateGroupIds;

internal sealed class UpdateGroupIdsCommandHandler(
    IClassRepository classRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateGroupIdsCommand>
{
    public async Task<Result> Handle(
        UpdateGroupIdsCommand request,
        CancellationToken cancellationToken)
    {
        var (classId, groupIds) = request;
        
        #region Get this Class 
        
        var classEntity = await classRepository.GetByIdAsync(
            classId,
            cancellationToken);
        if (classEntity == null)
        {
            return Result.Failure(
                DomainErrors.Class.NotFound(classId));
        }
        
        #endregion
        
        #region Update group ids

        var updateGroupIdsResult = classEntity.UpdateGroupIds(groupIds);
        if (updateGroupIdsResult.IsFailure)
        {
            return Result.Failure(
                updateGroupIdsResult.Error);
        }
        
        #endregion
        
        #region Update database
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}