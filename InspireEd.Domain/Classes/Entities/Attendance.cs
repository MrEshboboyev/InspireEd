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
    /// <param name="id">The unique identifier of the attendance record.</param>
    /// <param name="studentId">The unique identifier of the student.</param>
    /// <param name="classId">The unique identifier of the class.</param>
    /// <param name="status">The attendance status.</param>
    /// <param name="notes">Additional notes for the attendance record.</param>
    private Attendance(
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

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the unique identifier of the student.
    /// </summary>
    public Guid StudentId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the class.
    /// </summary>
    public Guid ClassId { get; set; }

    /// <summary>
    /// Gets or sets the attendance status.
    /// </summary>
    public AttendanceStatus Status { get; set; }

    /// <summary>
    /// Gets or sets additional notes for the attendance record.
    /// </summary>
    public string Notes { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the attendance record was created in UTC.
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the attendance record was last modified in UTC.
    /// </summary>
    public DateTime? ModifiedOnUtc { get; set; }

    #endregion
}