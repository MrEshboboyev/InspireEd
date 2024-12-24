namespace InspireEd.Domain.Users.Enums;

/// <summary> 
/// Represents different permissions that can be assigned to roles. 
/// </summary>
public enum Permission
{
    ReadUser = 1,
    UpdateUser = 2,
    AssignGroups = 3,
    AssignClasses = 4,
    ManageAttendance = 5
}