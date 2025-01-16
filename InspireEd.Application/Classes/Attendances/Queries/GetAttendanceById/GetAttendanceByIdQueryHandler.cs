using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Classes.Queries.Common;
using InspireEd.Domain.Classes.Repositories;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Shared;

namespace InspireEd.Application.Classes.Attendances.Queries.GetAttendanceById;

internal sealed class GetAttendanceByIdQueryHandler(
    IAttendanceRepository attendanceRepository) : IQueryHandler<GetAttendanceByIdQuery, AttendanceResponse>
{
    public async Task<Result<AttendanceResponse>> Handle(
        GetAttendanceByIdQuery request,
        CancellationToken cancellationToken)
    {
        var attendanceId = request.AttendanceId;
        
        #region Get this Attendance
        
        var attendance = await attendanceRepository.GetByIdAsync(
            attendanceId, cancellationToken);
        if (attendance == null)
        {
            return Result.Failure<AttendanceResponse>(
                DomainErrors.Attendance.NotFound(attendanceId));
        }
        
        #endregion
        
        #region Prepare response

        var attendanceResponse = AttendanceResponseFactory.Create(attendance);
        
        #endregion

        return Result.Success(attendanceResponse);
    }
}