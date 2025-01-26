using InspireEd.Domain.Shared;

namespace InspireEd.Domain.Errors;

/// <summary> 
/// Defines and organizes domain-specific errors. 
/// </summary>
public static class DomainErrors
{
    #region Users

    #region Entities

    public static class User
    {
        public static readonly Error EmailAlreadyInUse = new(
            "User.EmailAlreadyInUse",
            "The specified email is already in use");

        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "User.NotFound",
            $"The user with the identifier {id} was not found.");

        public static readonly Error NotExist = new(
            "Users.NotExist",
            $"There is no users");

        public static readonly Error InvalidCredentials = new(
            "User.InvalidCredentials",
            "The provided credentials are invalid");

        public static readonly Error InvalidPasswordChange = new(
            "User.InvalidPasswordChange",
            "The password change operation is invalid.");
        
        public static readonly Error InvalidRoleName = new(
            "User.InvalidRoleName",
            "The specified role is invalid.");

        public static readonly Func<int, Error> RoleNotAssigned = roleId => new Error(
            "User.RoleNotAssigned",
            $"The specified role with ID {roleId} is not assigned to the user.");

        public static readonly Func<int, Error> NoUsersFoundForRole = roleId => new Error(
            "User.NoUsersFoundForRole",
            $"No users found for the role with the identifier {roleId}.");

        public static readonly Func<string, Error> NotFoundForEmail = email => new Error(
            "User.NotFoundForEmail",
            $"The user with the email {email} was not found.");
        
        public static readonly Func<string, Error> NoUsersFoundForSearchTerm = searchTerm => new Error(
            "User.NoUsersFoundForSearchTerm",
            $"No users were found matching the search term '{searchTerm}'.");
    }

    public static class Role
    {
        public static readonly Func<int, Error> NotFound = id => new Error(
            "Role.NotFound",
            $"The role with the identifier {id} was not found.");

        public static readonly Error CannotBeNull = new Error(
            "Role.CannotBeNull",
            "The role cannot be null.");

        public static readonly Error NotAssignedToUser = new Error(
            "Role.NotAssignedToUser",
            "The role is not assigned to this user.");
    }

    #endregion

    #region Value Objects

    public static class Email
    {
        public static readonly Error Empty = new(
            "Email.Empty",
            "Email is empty");

        public static readonly Error InvalidFormat = new(
            "Email.InvalidFormat",
            "Email format is invalid");
    }

    public static class FirstName
    {
        public static readonly Error Empty = new(
            "FirstName.Empty",
            "First name is empty");

        public static readonly Error TooLong = new(
            "LastName.TooLong",
            "FirstName name is too long");
    }

    public static class LastName
    {
        public static readonly Error Empty = new(
            "LastName.Empty",
            "Last name is empty");

        public static readonly Error TooLong = new(
            "LastName.TooLong",
            "Last name is too long");
    }

    #endregion

    #region Roles

    public static class Teacher
    {
        public static readonly Error EmailAlreadyInUse = new(
            "User.EmailAlreadyInUse",
            "The specified email is already in use");

        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Teacher.NotFound",
            $"The teacher with the identifier {id} was not found.");

        public static readonly Error NotExist = new(
            "Teachers.NotExist",
            $"There is no teachers");

        public static readonly Error InvalidCredentials = new(
            "Teacher.InvalidCredentials",
            "The provided credentials are invalid");
    }

    public static class DepartmentHead
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "DepartmentHead.NotFound",
            $"The department head with the identifier {id} was not found.");
    }

    public static class Student
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Student.NotFound",
            $"The student with the identifier {id} was not found.");
    }

    #endregion

    #endregion

    #region Faculties

    #region Entities

    public static class Faculty
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Faculty.NotFound",
            $"The faculty with the identifier {id} was not found.");

        public static readonly Error NotExist = new(
            "Faculties.NotExist",
            "There is no faculties");

        public static readonly Func<Guid, Error> DepartmentHeadIdAlreadyExists = id => new Error(
            "Faculty.DepartmentHeadIdAlreadyExists",
            $"The department head with the identifier {id} is already assigned.");

        public static readonly Func<Guid, Error> DepartmentHeadIdDoesNotExist = id => new Error(
            "Faculty.DepartmentHeadIdDoesNotExist",
            $"The department head with the identifier {id} does not exist.");

        public static readonly Func<string, Error> GroupNameAlreadyExists = groupName => new Error(
            "Faculty.GroupNameAlreadyExists",
            $"The group name '{groupName}' is already in use.");

        public static readonly Func<Guid, Error> GroupDoesNotExist = groupId => new Error(
            "Faculty.GroupDoesNotExist",
            $"The group with the identifier {groupId} does not exist.");

        public static readonly Error SomeGroupsNotFound = new Error(
            "Faculty.SomeGroupsNotFound",
            "Some of the specified groups were not found.");

        public static readonly Error MergeGroupCountInsufficient = new Error(
            "Faculty.MergeGroupCountInsufficient",
            "At least two groups are required to merge.");

        public static readonly Error InvalidSplitGroupParameters = new Error(
            "Faculty.InvalidSplitGroupParameters",
            "Invalid parameters for splitting the group.");
    }

    public static class Group
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Group.NotFound",
            $"The group with the identifier {id} was not found.");

        public static readonly Error NotExist = new(
            "Group.NotExist",
            $"There is no groups");

        public static readonly Func<Guid, Guid, Error> StudentNotExist = (groupId, studentId) => new Error(
            "Group.StudentNotExist",
            $"The student with the identifier {studentId} does not" +
            $" exist in the group with the identifier {groupId}.");

        public static readonly Func<IEnumerable<Guid>, Error> MissingGroups = ids => new Error(
            "Group.MissingGroups",
            $"The following groups are missing: {string.Join(", ", ids)}");
    }

    #endregion

    #region Value Objects

    public static class FacultyName
    {
        public static readonly Error Empty = new(
            "FacultyName.Empty",
            "Faculty Name is empty");

        public static readonly Error TooLong = new(
            "FacultyName.TooLong",
            "Faculty name name is too long");
    }

    public static class GroupName
    {
        public static readonly Error Empty = new(
            "GroupName.Empty",
            "Group Name is empty");

        public static readonly Error TooLong = new(
            "GroupName.TooLong",
            "Group name is too long");
    }

    #endregion

    #endregion

    #region Subjects

    #region Entities

    public static class Subject
    {
        public static readonly Error EmailAlreadyInUse = new(
            "User.EmailAlreadyInUse",
            "The specified email is already in use");

        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Subject.NotFound",
            $"The subject with the identifier {id} was not found.");

        public static readonly Error NotExist = new(
            "Subjects.NotExist",
            $"There is no subjects");

        public static readonly Error InvalidCredentials = new(
            "User.InvalidCredentials",
            "The provided credentials are invalid");
    }

    #endregion

    #region Value Objects

    public static class SubjectCode
    {
        public static readonly Error Empty = new(
            "SubjectCode.Empty",
            "Subject Code is empty");

        public static readonly Error TooLong = new(
            "SubjectCode.TooLong",
            "Subject code is too long");
    }

    public static class SubjectName
    {
        public static readonly Error Empty = new(
            "SubjectName.Empty",
            "Subject Name is empty");

        public static readonly Error TooLong = new(
            "SubjectName.TooLong",
            "Subject name is too long");
    }

    public static class SubjectCredit
    {
        public static readonly Error InvalidCredit = new(
            "SubjectCredit.InvalidCredit",
            "Subject credit must be greater than zero.");

        public static readonly Error TooHigh = new(
            "SubjectCredit.TooHigh",
            $"Subject credit must not exceed {Subjects.ValueObjects.SubjectCredit.MaxCredit}.");
    }

    #endregion

    #endregion

    #region Classes

    #region Entities

    public static class Class
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Class.NotFound",
            $"The class with the identifier {id} was not found.");

        public static readonly Func<Guid, List<Guid>, Error> StudentNotExist = (classId, studentIds) => new Error(
            "Class.StudentNotExist",
            $"The following students with identifiers {string.Join(", ", studentIds)} do not exist in the class with the identifier {classId}.");

        public static readonly Func<Guid, Error> NotFoundBySubjectId = subjectId => new Error(
            "Class.NotFoundBySubjectId",
            $"No classes found for the subject with the identifier {subjectId}.");
    }

    public static class Attendance
    {
        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Attendance.NotFound",
            $"The attendance record with the identifier {id} was not found.");
    }

    #endregion

    #endregion
}