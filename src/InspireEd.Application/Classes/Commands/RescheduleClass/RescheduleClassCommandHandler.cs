using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Classes.Commands.RescheduleClass;

internal sealed class RescheduleClassCommandHandler(
    IClassRepository classRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<RescheduleClassCommand>
{
    public async Task<Result> Handle(
        RescheduleClassCommand request,
        CancellationToken cancellationToken)
    {
        var (classId, newScheduledDate) = request;
        
        #region Get this Class
        
        var classEntity = await classRepository.GetByIdAsync(
            classId,
            cancellationToken);
        if (classEntity is null)
        {
            return Result.Failure(
                DomainErrors.Class.NotFound(classId));
        }
        
        #endregion
        
        #region Reschedule Class

        var rescheduleResult = classEntity.Reschedule(newScheduledDate);
        if (rescheduleResult.IsFailure)
        {
            return Result.Failure(
                rescheduleResult.Error);
        }
        
        #endregion

        #region Update database
        
        classRepository.Update(classEntity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}