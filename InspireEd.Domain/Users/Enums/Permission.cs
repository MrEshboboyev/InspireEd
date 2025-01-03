namespace InspireEd.Domain.Users.Enums;

/// <summary> 
/// Represents different permissions that can be assigned to roles. 
/// </summary>
public enum Permission
{
    // General User Permissions
    ViewProfile = 10,          // View personal information (name, group, etc.)
    ViewAttendance = 20,       // View attendance records

    // Teacher Permissions
    ManageAttendance = 30,     // Mark attendance for assigned lessons
    ViewAssignedClasses = 40,  // View the list of assigned classes/groups

    // Department Head Permissions
    AddStudents = 50,          // Add new students to the system
    AssignGroups = 60,         // Assign students to groups
    AssignClasses = 70,        // Assign teachers to classes
    ViewFacultyData = 80,      // View all students and teachers in the faculty
    ManageGroups = 90,

    // Admin Permissions
    FullAccess = 100            // Unrestricted access to all system resources
}
