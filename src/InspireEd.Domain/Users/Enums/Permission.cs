namespace InspireEd.Domain.Users.Enums;

/// <summary> 
/// Represents different permissions that can be assigned to roles. 
/// </summary>
public enum Permission
{
    #region Student
    
    // General User Permissions
    ViewProfile = 10,          // View personal information (name, group, etc.)
    ViewAttendance = 20,       // View attendance records
    
    #endregion

    #region Teacher 
    
    // Teacher Permissions
    CreateAttendances = 30,     // Mark attendance for assigned lessons
    ViewAssignedClasses = 40,  // View the list of assigned classes/groups
    
    #endregion

    #region Department Head
    
    // Department Head Permissions
    #region Students
    
    AddStudents = 50,          // Add new students to the system
    RemoveStudents = 51,          // Remove students from the system
    TransferStudentBetweenGroups = 52,          // Transfer students between groups in the system
    ViewStudents = 53,
    
    #endregion
    
    #region Groups
    
    ManageGroups = 60,
    AssignGroups = 61,         // Assign students to groups
    
    #endregion
    
    #region Classes
    
    AssignClasses = 70,        // Assign teachers to classes
    
    #region Attendances
    
    UpdateAttendances = 75,
    DeleteAttendances = 76,
    
    #endregion
    
    #endregion
    
    #region Faculties
    
    ViewFacultyData = 80,      // View all students and teachers in the faculty
    
    #endregion
    
    #region Subjects
    
    ViewSubjects = 90,
    ManageSubjects = 95,
    
    #endregion
    
    #endregion

    #region Admin
    
    // Admin Permissions
    FullAccess = 100            // Unrestricted access to all system resources
    
    #endregion
}
