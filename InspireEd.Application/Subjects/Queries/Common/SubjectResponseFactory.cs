using InspireEd.Domain.Subjects.Entities;

namespace InspireEd.Application.Subjects.Queries.Common;

public static class SubjectResponseFactory
{
    public static SubjectResponse Create(Subject subject)
    {
        return new SubjectResponse(
            subject.Id,
            subject.Name.Value,
            subject.Code.Value,
            subject.Credit.Value,
            subject.CreatedOnUtc,
            subject.ModifiedOnUtc);
    }
}