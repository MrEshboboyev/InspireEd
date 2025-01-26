using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Classes.Commands.CreateAttendances;

internal sealed class CreateAttendancesCommandHandler(
    IClassRepository classRepository,
    IAttendanceRepository attendanceRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<CreateAttendancesCommand>
{
    public async Task<Result> Handle(
        CreateAttendancesCommand request,
        CancellationToken cancellationToken)
    {
        var (classId, attendances) = request;

        #region Get Class and related group student ids

        // get Class
        var classEntity = await classRepository.GetByIdAsync(
            classId,
            cancellationToken);
        if (classEntity is null)
        {
            return Result.Failure(
                DomainErrors.Class.NotFound(classId));
        }

        // get StudentIds
        var groupStudentIds = await classRepository.GetGroupStudentIds(
            classId,
            cancellationToken);

        #endregion

        #region Checking request student ids is equal class related group students ids

        var requestStudentIds = attendances
            .Select(a => a.StudentId)
            .ToList();
        if (!requestStudentIds.All(id => groupStudentIds.Contains(id)))
        {
            return Result.Failure(
                DomainErrors.Class.StudentNotExist(classId, requestStudentIds));
        }

        #endregion

        #region Add new attendances to class and db

        foreach (var addAttendanceResult in attendances
                     .Select(attendance =>
                         classEntity.AddAttendance(
                             attendance.StudentId,
                             attendance.Status,
                             attendance.Notes)))
        {
            if (addAttendanceResult.IsFailure)
            {
                return Result.Failure(
                    addAttendanceResult.Error);
            }

            attendanceRepository.Add(addAttendanceResult.Value);
        }

        #endregion

        #region Update database

        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}