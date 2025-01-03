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
        
        public static readonly Error InvalidRoleName = new Error(
            "User.InvalidRoleName",
            "The provided role name is invalid.");
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
}