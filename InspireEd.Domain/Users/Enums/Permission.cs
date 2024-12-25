namespace InspireEd.Domain.Users.Enums;

/// <summary> 
/// Represents different permissions that can be assigned to roles. 
/// </summary>
public enum Permission
{
    // General User Permissions
    ViewProfile = 1,          // View personal information (name, group, etc.)
    ViewAttendance = 2,       // View attendance records

    // Teacher Permissions
    ManageAttendance = 3,     // Mark attendance for assigned lessons
    ViewAssignedClasses = 4,  // View the list of assigned classes/groups

    // Department Head Permissions
    AddStudents = 5,          // Add new students to the system
    AssignGroups = 6,         // Assign students to groups
    AssignClasses = 7,        // Assign teachers to classes
    ViewFacultyData = 8,      // View all students and teachers in the faculty

    // Admin Permissions
    FullAccess = 9            // Unrestricted access to all system resources
}
