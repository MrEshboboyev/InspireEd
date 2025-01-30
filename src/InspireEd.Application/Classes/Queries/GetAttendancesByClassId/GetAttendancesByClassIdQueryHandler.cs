using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Classes.Queries.Common;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Classes.Queries.GetAttendancesByClassId;

internal sealed class GetAttendancesByClassIdQueryHandler(
    IClassRepository classRepository) : IQueryHandler<GetAttendancesByClassIdQuery, List<AttendanceResponse>>
{
    public async Task<Result<List<AttendanceResponse>>> Handle(
        GetAttendancesByClassIdQuery request,
        CancellationToken cancellationToken)
    {
        var classId = request.ClassId;
        
        #region Get this class attendances
        
        var classEntity = await classRepository.GetByIdWithAttendancesAsync(
            classId,
            cancellationToken);
        if (classEntity is null)
        {
            return Result.Failure<List<AttendanceResponse>>(
                DomainErrors.Class.NotFound(classId));
        }
        
        #endregion
        
        #region Prepare response

        var attendanceResponses = classEntity
            .Attendances
            .Select(AttendanceResponseFactory.Create)
            .ToList();
        
        #endregion

        return Result.Success(attendanceResponses);
    }
}