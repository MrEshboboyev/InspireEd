using InspireEd.Domain.Faculties.Entities;

namespace InspireEd.Application.Faculties.Queries.Common;

public static class GroupResponseFactory
{
    public static GroupResponse Create(Group group)
    {
        return new GroupResponse(
            group.Id,
            group.Name.Value,
            group.StudentIds,
            group.CreatedOnUtc,
            group.ModifiedOnUtc);
    }
}