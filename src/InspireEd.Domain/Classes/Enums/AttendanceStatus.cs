namespace InspireEd.Domain.Classes.Enums;

/// <summary>
/// Defines the possible attendance statuses for a class.
/// </summary>
public enum AttendanceStatus
{
    /// <summary>
    /// Indicates that the student is present.
    /// </summary>
    Present = 10,

    /// <summary>
    /// Indicates that the student is absent.
    /// </summary>
    Absent = 20,

    /// <summary>
    /// Indicates that the student is excused.
    /// </summary>
    Excused = 30
}