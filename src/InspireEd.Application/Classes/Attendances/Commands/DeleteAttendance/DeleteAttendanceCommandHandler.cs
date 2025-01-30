using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Classes.Attendances.Commands.DeleteAttendance;

internal sealed class DeleteAttendanceCommandHandler(
    IClassRepository classRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<DeleteAttendanceCommand>
{
    public async Task<Result> Handle(
        DeleteAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        var (classId, attendanceId) = request;
        
        #region Get Class and its attendance
        
        var classEntity = await classRepository.GetByIdWithAttendancesAsync(
            classId,
            cancellationToken);
        if (classEntity is null)
        {
            return Result.Failure(
                DomainErrors.Class.NotFound(classId));
        }

        var attendance = classEntity.Attendances
            .FirstOrDefault(a => a.Id == attendanceId);
        if (attendance is null)
        {
            return Result.Failure(
                DomainErrors.Attendance.NotFound(attendanceId));
        }
        
        #endregion

        #region Remove attendance from class
        
        var removeAttendanceResult = classEntity.RemoveAttendance(attendance);
        if (removeAttendanceResult.IsFailure)
        {
            return Result.Failure(
                removeAttendanceResult.Error);
        }
        
        #endregion
        
        #region Update database
        
        classRepository.Update(classEntity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        #endregion

        return Result.Success();
    }
}