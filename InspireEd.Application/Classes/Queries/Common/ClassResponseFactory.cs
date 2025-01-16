using InspireEd.Application.Classes.Queries.GetClassById;
using InspireEd.Domain.Classes.Entities;

namespace InspireEd.Application.Classes.Queries.Common;

public static class ClassResponseFactory
{
    public static ClassResponse Create(Class classEntity)
    {
        return new ClassResponse(
            classEntity.Id,
            classEntity.SubjectId,
            classEntity.TeacherId,
            classEntity.Type,
            classEntity.GroupIds,
            classEntity.ScheduledDate,
            classEntity.Attendances
                .Select(AttendanceResponseFactory.Create)
                .ToList());
    }
}