using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Classes.Queries.Common;

namespace InspireEd.Application.Classes.Attendances.Queries.GetAttendanceById;

public sealed record GetAttendanceByIdQuery(
    Guid AttendanceId) : IQuery<AttendanceResponse>;