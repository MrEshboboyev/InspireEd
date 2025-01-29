using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Repositories;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Faculties.Repositories;

namespace InspireEd.Application.Classes.Commands.CreateAttendances;

internal sealed class CreateAttendancesCommandHandler(
    IClassRepository classRepository,
    IAttendanceRepository attendanceRepository,
    IUnitOfWork unitOfWork,
    IGroupRepository groupRepository) : ICommandHandler<CreateAttendancesCommand>
{
    public async Task<Result> Handle(
        CreateAttendancesCommand request,
        CancellationToken cancellationToken)
    {
        var (classId, teacherId, attendances) = request;

        #region Get Class and related group student ids

        // Get Class
        var classEntity = await classRepository.GetByIdAsync(
            classId,
            cancellationToken);
        if (classEntity is null)
        {
            return Result.Failure(
                DomainErrors.Class.NotFound(classId));
        }

        if (classEntity.TeacherId != teacherId)
        {
            return Result.Failure(
                DomainErrors.Teacher.NotAssignedToClass(teacherId, classId));
        }

        // Get Group StudentIds associated with the Class
        var groupStudentIds = await groupRepository.GetStudentIdsForGroupsAsync(
            classEntity.GroupIds, // Using GroupIds from the Class entity
            cancellationToken);

        #endregion

        #region Checking if request student ids are valid

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

        classRepository.Update(classEntity);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        return Result.Success();
    }
}
