using InspireEd.Domain.Classes.Enums;
using InspireEd.Domain.Primitives;

namespace InspireEd.Domain.Classes.Entities;

/// <summary>
/// Represents an attendance record for a class.
/// </summary>
public sealed class Attendance : Entity, IAuditableEntity
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Attendance"/> class with the specified parameters.
    /// </summary>
    internal Attendance(
        Guid id,
        Guid studentId,
        Guid classId,
        AttendanceStatus status,
        string notes = "") : base(id)
    {
        StudentId = studentId;
        ClassId = classId;
        Status = status;
        Notes = notes;
    }

    // EF Core requires a parameterless constructor
    private Attendance() { }

    #endregion

    #region Properties

    /// <summary> Gets the unique identifier of the student. </summary>
    public Guid StudentId { get; private set; }

    /// <summary> Gets the unique identifier of the class. </summary>
    public Guid ClassId { get; private set; }

    /// <summary> Navigation property to Class. </summary>
    public Class Class { get; private set; } = null!; // ✅ Ensures EF Core recognizes this navigation property.

    /// <summary> Gets the attendance status. </summary>
    public AttendanceStatus Status { get; private set; }

    /// <summary> Gets additional notes for the attendance record. </summary>
    public string Notes { get; private set; }

    /// <summary> Gets the date and time when the attendance record was created in UTC. </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary> Gets the date and time when the attendance record was last modified in UTC. </summary>
    public DateTime? ModifiedOnUtc { get; set; }

    #endregion

    #region Methods

    public void UpdateStatus(AttendanceStatus status, string notes)
    {
        Status = status;
        Notes = notes;
        ModifiedOnUtc = DateTime.UtcNow;
    }

    #endregion
}
