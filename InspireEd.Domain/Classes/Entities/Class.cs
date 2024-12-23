using InspireEd.Domain.Classes.Enums;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Primitives;
using InspireEd.Domain.Subjects.ValueObjects;

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
    private readonly List<GroupName> _groups = [];

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Class"/> class with the specified parameters.
    /// </summary>
    /// <param name="id">The unique identifier of the class.</param>
    /// <param name="subject">The subject code of the class.</param>
    /// <param name="teacherId">The unique identifier of the teacher.</param>
    /// <param name="type">The type of the class.</param>
    /// <param name="groups">The list of group names associated with the class.</param>
    /// <param name="scheduledDate">The scheduled date of the class.</param>
    private Class(
        Guid id,
        SubjectCode subject,
        Guid teacherId,
        ClassType type,
        List<GroupName> groups,
        DateTime scheduledDate) : base(id)
    {
        Subject = subject;
        TeacherId = teacherId;
        Type = type;
        Groups = groups;
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
    public SubjectCode Subject { get; set; }

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
    public List<GroupName> Groups { get; set; }

    /// <summary>
    /// Gets the read-only collection of attendances associated with the class.
    /// </summary>
    public IReadOnlyCollection<Attendance> Attendances => _attendances.AsReadOnly();

    #endregion
}