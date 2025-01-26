using InspireEd.Application.Abstractions.Messaging;
using InspireEd.Application.Classes.Queries.Common;

namespace InspireEd.Application.Classes.Queries.GetAttendancesByClassId;

public sealed record GetAttendancesByClassIdQuery(
    Guid ClassId) : IQuery<List<AttendanceResponse>>;