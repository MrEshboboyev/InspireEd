using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Primitives;
using InspireEd.Domain.Users.Entities;

namespace InspireEd.Domain.Faculties.Entities;

/// <summary>
/// Represents a group entity within a faculty.
/// </summary>
public sealed class Group : Entity, IAuditableEntity
{
    #region Private Fields

    /// <summary>
    /// A list of students associated with the group.
    /// </summary>
    private readonly List<Student> _students = [];

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Group"/> class with the specified ID, faculty ID, and name.
    /// </summary>
    /// <param name="id">The unique identifier of the group.</param>
    /// <param name="facultyId">The unique identifier of the faculty to which the group belongs.</param>
    /// <param name="name">The name of the group.</param>
    internal Group(
        Guid id,
        Guid facultyId,
        GroupName name) : base(id)
    {
        FacultyId = facultyId;
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Group"/> class.
    /// </summary>
    private Group()
    {
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the unique identifier of the faculty to which the group belongs.
    /// </summary>
    public Guid FacultyId { get; set; }

    /// <summary>
    /// Gets or sets the name of the group.
    /// </summary>
    public GroupName Name { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the group was created in UTC.
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the group was last modified in UTC.
    /// </summary>
    public DateTime? ModifiedOnUtc { get; set; }

    #endregion
}