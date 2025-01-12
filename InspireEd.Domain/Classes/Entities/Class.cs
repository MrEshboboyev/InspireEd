using InspireEd.Domain.Classes.Enums;
using InspireEd.Domain.Errors;
using InspireEd.Domain.Primitives;
using InspireEd.Domain.Shared;

namespace InspireEd.Domain.Classes.Entities;

/// <summary>
/// Represents a class entity.
/// </summary>
public sealed class Class : AggregateRoot
{
    #region Private Fields

    /// <summary>
    /// A list of attendances associated with the class.
    /// </summary>
    private readonly List<Attendance> _attendances = [];

    /// <summary>
    /// A list of group names associated with the class.
    /// </summary>
    private readonly List<Guid> _groupIds = [];

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Class"/> class with the specified parameters.
    /// </summary>
    /// <param name="id">The unique identifier of the class.</param>
    /// <param name="subjectId">The subject id of the class.</param>
    /// <param name="teacherId">The unique identifier of the teacher.</param>
    /// <param name="type">The type of the class.</param>
    /// <param name="groupIds">The list of group ids associated with the class.</param>
    /// <param name="scheduledDate">The scheduled date of the class.</param>
    private Class(
        Guid id,
        Guid subjectId,
        Guid teacherId,
        ClassType type,
        List<Guid> groupIds,
        DateTime scheduledDate) : base(id)
    {
        SubjectId = subjectId;
        TeacherId = teacherId;
        Type = type;
        _groupIds = groupIds;
        ScheduledDate = scheduledDate;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Class"/> class.
    /// </summary>
    private Class()
    {
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the subject code of the class.
    /// </summary>
    public Guid SubjectId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the teacher.
    /// </summary>
    public Guid TeacherId { get; set; }

    /// <summary>
    /// Gets or sets the type of the class.
    /// </summary>
    public ClassType Type { get; set; }

    /// <summary>
    /// Gets or sets the scheduled date of the class.
    /// </summary>
    public DateTime ScheduledDate { get; set; }

    /// <summary>
    /// Gets or sets the list of group names associated with the class.
    /// </summary>
    public IReadOnlyCollection<Guid> GroupIds => _groupIds.AsReadOnly();

    /// <summary>
    /// Gets the read-only collection of attendances associated with the class.
    /// </summary>
    public IReadOnlyCollection<Attendance> Attendances => _attendances.AsReadOnly();

    #endregion
    
    #region Factory methods

    public static Class Create(
        Guid id,
        Guid subjectId,
        Guid teacherId,
        ClassType type,
        List<Guid> groupIds,
        DateTime scheduledDate)
    {
        return new Class(
            id,
            subjectId,
            teacherId,
            type,
            groupIds,
            scheduledDate);
    }
    
    #endregion
    
    #region Own Methods
    
    public Result UpdateClassDetails(
        Guid subjectId,
        Guid teacherId,
        ClassType type,
        DateTime scheduledDate)
    {
        #region Update fields
        
        SubjectId = subjectId;
        TeacherId = teacherId;
        Type = type;
        ScheduledDate = scheduledDate;
        
        #endregion
        
        return Result.Success();
    }
    
    #endregion
    
    #region Attendance methods

    public Result<Attendance> AddAttendance(
        Guid studentId,
        AttendanceStatus attendanceStatus,
        string notes)
    {
        var attendance = new Attendance(
            Guid.NewGuid(),
            studentId,
            this.Id,
            attendanceStatus,
            notes);
    
        _attendances.Add(attendance);
        return Result.Success(attendance);
    }
    
    public Result<Attendance> UpdateAttendance(
        Guid attendanceId, 
        AttendanceStatus status,
        string notes)
    {
        var attendance = _attendances
            .FirstOrDefault(a => a.Id == attendanceId);
        if (attendance == null)
        {
            return Result.Failure<Attendance>(
                DomainErrors.Attendance.NotFound(attendanceId));
        }
    
        attendance.UpdateStatus(status, notes);
        return Result.Success(attendance);
    }
    
    #endregion
}