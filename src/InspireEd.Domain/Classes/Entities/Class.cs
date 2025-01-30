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

    private readonly List<Attendance> _attendances = [];
    private readonly List<Guid> _groupIds = [];

    #endregion

    #region Constructors

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

    private Class() { }

    #endregion

    #region Properties

    public Guid SubjectId { get; private set; }
    public Guid TeacherId { get; private set; }
    public ClassType Type { get; private set; }
    public DateTime ScheduledDate { get; private set; }

    /// <summary> Navigation property to Attendance. </summary>
    public IReadOnlyCollection<Attendance> Attendances => _attendances.AsReadOnly();

    public IReadOnlyCollection<Guid> GroupIds => _groupIds.AsReadOnly();

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

        if (scheduledDate < DateTime.UtcNow)
        {
            return Result.Failure(
                DomainErrors.Class.InvalidScheduledDate);
        }
        
        SubjectId = subjectId;
        TeacherId = teacherId;
        Type = type;
        ScheduledDate = scheduledDate;
        
        #endregion
        
        return Result.Success();
    }

    public Result UpdateGroups(List<Guid> groupIds)
    {
        #region Update group ids
    
        _groupIds.Clear();
        _groupIds.AddRange(groupIds);

        #endregion

        return Result.Success();
    }

    public Result Reschedule(DateTime newScheduledDate)
    {
        #region Reschedule fields
        
        if (newScheduledDate < DateTime.UtcNow)
        {
            return Result.Failure(
                DomainErrors.Class.InvalidScheduledDate);
        }
        ScheduledDate = newScheduledDate;
        
        #endregion
        
        return Result.Success();
    }
    
    public Result ChangeTeacher(Guid newTeacherId)
    {
        #region Update fields
        
        TeacherId = newTeacherId;
        
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
            Id,
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
        #region Checking this Attendace exist
        
        var attendance = _attendances
            .FirstOrDefault(a => a.Id == attendanceId);
        if (attendance is null)
        {
            return Result.Failure<Attendance>(
                DomainErrors.Attendance.NotFound(attendanceId));
        }
        
        #endregion
        
        #region Update this Attendance
    
        attendance.UpdateStatus(status, notes);
        
        #endregion
        
        return Result.Success(attendance);
    }
    
    public Result RemoveAttendance(Attendance attendance)
    {
        #region Remove attendance
        
        _attendances.Remove(attendance);
        
        #endregion
        
        return Result.Success();
    }

    #endregion
}