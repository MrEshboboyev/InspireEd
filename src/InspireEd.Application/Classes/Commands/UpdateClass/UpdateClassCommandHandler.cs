using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Classes.Commands.UpdateClass;

internal sealed class UpdateClassCommandHandler(
    IClassRepository classRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateClassCommand>
{
    public async Task<Result> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
    {
        var (classId, subjectId, 
            teacherId, classType, scheduledDate) = request;
        
        #region Get Class
        
        var classEntity = await classRepository.GetByIdAsync(classId, cancellationToken);
        if (classEntity is null)
        {
            return Result.Failure(
                DomainErrors.Class.NotFound(request.ClassId));
        }
        
        #endregion
        
        #region Update this Class

        var updateClassResult = classEntity.UpdateClassDetails(
            subjectId, 
            teacherId, 
            classType, 
            scheduledDate);
        if (updateClassResult.IsFailure)
        {
            return Result.Failure(
                updateClassResult.Error);
        }
        
        #endregion
        
        #region Update database
        
        classRepository.Update(classEntity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}