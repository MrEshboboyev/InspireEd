using InspireEd.Domain.Users.Entities;

namespace InspireEd.Application.Faculties.Groups.Queries.Common;

public static class StudentResponseFactory
{
    public static StudentResponse Create(User student)
    {
        return new StudentResponse(
            student.Id,
            student.FirstName.Value,
            student.LastName.Value,
            student.Email.Value,
            student.CreatedOnUtc,
            student.ModifiedOnUtc);
    }
}