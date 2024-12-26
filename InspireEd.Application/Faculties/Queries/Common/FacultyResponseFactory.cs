using InspireEd.Domain.Faculties.Entities;

namespace InspireEd.Application.Faculties.Queries.Common;

public static class FacultyResponseFactory
{
    public static FacultyResponse Create(Faculty faculty)
    {
        return new FacultyResponse(
            faculty.Id,
            faculty.Name.Value,
            faculty.CreatedOnUtc,
            faculty.ModifiedOnUtc);
    }
}