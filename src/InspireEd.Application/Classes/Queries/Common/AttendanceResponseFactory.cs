using InspireEd.Application.Classes.Queries.GetClassById;
using InspireEd.Domain.Classes.Entities;

namespace InspireEd.Application.Classes.Queries.Common;

public static class AttendanceResponseFactory
{
    public static AttendanceResponse Create(Attendance attendance)
    {
        return new AttendanceResponse(
            attendance.Id,
            attendance.StudentId,
            attendance.Status,
            attendance.Notes,
            attendance.CreatedOnUtc,
            attendance.ModifiedOnUtc);
    }
}