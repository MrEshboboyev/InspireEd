using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Classes.Attendances.Commands.UpdateAttendance;

internal sealed class UpdateAttendanceCommandHandler(
    IClassRepository classRepository,
    IAttendanceRepository attendanceRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<UpdateAttendanceCommand>
{
    public async Task<Result> Handle(UpdateAttendanceCommand request, CancellationToken cancellationToken)
    {
        var (classId, attendanceId, attendanceStatus, notes) = request;
        
        #region Get this Class
        
        var classEntity = await classRepository.GetByIdAsync(
            classId,
            cancellationToken);
        if (classEntity is null)
        {
            return Result.Failure(DomainErrors.Class.NotFound(classId));
        }
        
        #endregion

        #region Update attendance in Class
        
        var updateAttendanceResult = classEntity.UpdateAttendance(
            attendanceId,
            attendanceStatus,
            notes);
        if (updateAttendanceResult.IsFailure)
        {
            return Result.Failure(
                updateAttendanceResult.Error);
        }
        
        #endregion
        
        #region Update database
        
        attendanceRepository.Update(updateAttendanceResult.Value);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion
        
        return Result.Success();
    }
}