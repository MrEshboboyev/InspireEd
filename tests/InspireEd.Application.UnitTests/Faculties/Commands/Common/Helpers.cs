using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.ValueObjects;

namespace InspireEd.Application.UnitTests.Faculties.Commands.Common;

public static class Helpers
{
    public static Faculty CreateTestFaculty(Guid id, string facultyName)
    {
        var facultyNameObj = FacultyName.Create(facultyName).Value;

        return Faculty.Create(id, facultyNameObj);
    }
}