using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Primitives;

namespace InspireEd.Domain.Faculties.Entities;

/// <summary>
/// Represents a faculty entity.
/// </summary>
public sealed class Faculty : AggregateRoot, IAuditableEntity
{
    #region Private Fields

    /// <summary>
    /// A list of groups associated with the faculty.
    /// </summary>
    private readonly List<Group> _groups = [];

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Faculty"/> class with the specified ID and name.
    /// </summary>
    /// <param name="id">The unique identifier of the faculty.</param>
    /// <param name="name">The name of the faculty.</param>
    private Faculty(
        Guid id,
        FacultyName name) : base(id)
    {
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Faculty"/> class.
    /// </summary>
    private Faculty()
    {
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the name of the faculty.
    /// </summary>
    public FacultyName Name { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the faculty was created in UTC.
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the faculty was last modified in UTC.
    /// </summary>
    public DateTime? ModifiedOnUtc { get; set; }

    #endregion
}