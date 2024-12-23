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
        public static readonly Error EmailAlreadyInUse = new(
            "User.EmailAlreadyInUse",
            "The specified email is already in use");

        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Faculty.NotFound",
            $"The faculty with the identifier {id} was not found.");

        public static readonly Error NotExist = new(
            "Faculties.NotExist",
            $"There is no faculties");

        public static readonly Error InvalidCredentials = new(
            "User.InvalidCredentials",
            "The provided credentials are invalid");
    }
    
    public static class Group
    {
        public static readonly Error EmailAlreadyInUse = new(
            "User.EmailAlreadyInUse",
            "The specified email is already in use");

        public static readonly Func<Guid, Error> NotFound = id => new Error(
            "Group.NotFound",
            $"The group with the identifier {id} was not found.");

        public static readonly Error NotExist = new(
            "Group.NotExist",
            $"There is no groups");

        public static readonly Error InvalidCredentials = new(
            "User.InvalidCredentials",
            "The provided credentials are invalid");
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
            "Group name name is too long");
    }
    
    #endregion
    
    #endregion
}

