using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Classes.Enums;
using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Subjects.Entities;
using InspireEd.Domain.Subjects.ValueObjects;
using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.ValueObjects;

namespace InspireEd.Application.UnitTests.Common;

public static class Helpers
{
    public static Faculty CreateTestFaculty(Guid id, string facultyName)
    {
        var facultyNameObj = FacultyName.Create(facultyName).Value;

        return Faculty.Create(id, facultyNameObj);
    }

    // Helper method to create a User instance
    public static User CreateTestUser(Guid id, string email, string passwordHash, string firstName, string lastName,
        string roleName)
    {
        var emailObj = Email.Create(email).Value;
        var firstNameObj = FirstName.Create(firstName).Value;
        var lastNameObj = LastName.Create(lastName).Value;
        var role = Role.FromName(roleName);

        return User.Create(id, emailObj, passwordHash, firstNameObj, lastNameObj, role);
    }

    // Helper method to create a Role instance
    public static Role CreateTestRole(int id, string name)
    {
        return new Role(id, name);
    }

    public static Subject CreateTestSubject(
        Guid id,
        string subjectName,
        string subjectCode,
        int subjectCredit) =>
        Subject.Create(
            id,
            SubjectName.Create(subjectName).Value,
            SubjectCode.Create(subjectCode).Value,
            SubjectCredit.Create(subjectCredit).Value);

    public static Class CreateTestClass(
        Guid classId,
        Guid subjectId,
        Guid teacherId,
        ClassType classType,
        List<Guid> groupIds,
        DateTime scheduledDate) =>
        Class.Create(
            classId,
            subjectId,
            teacherId,
            classType,
            groupIds,
            scheduledDate);
}