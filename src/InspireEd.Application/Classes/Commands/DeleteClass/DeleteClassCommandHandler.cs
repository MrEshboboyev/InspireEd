using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Classes.Commands.DeleteClass;

internal sealed class DeleteClassCommandHandler(
    IClassRepository classRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteClassCommand>
{
    public async Task<Result> Handle(
        DeleteClassCommand request,
        CancellationToken cancellationToken)
    {
        var classId = request.ClassId;
        
        #region Get this class
        
        var classEntity = await classRepository.GetByIdAsync(
            classId,
            cancellationToken);
        
        if (classEntity is null)
        {
            return Result.Failure(
                DomainErrors.Class.NotFound(classId));
        }
        
        #endregion
        
        #region Delete and update database

        classRepository.Remove(classEntity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion
        
        return Result.Success();
    }
}