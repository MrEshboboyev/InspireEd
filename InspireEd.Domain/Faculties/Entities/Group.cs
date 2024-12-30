using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Primitives;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Entities;

namespace InspireEd.Domain.Faculties.Entities;

/// <summary>
/// Represents a group entity within a faculty.
/// </summary>
public sealed class Group : Entity, IAuditableEntity
{
    #region Private Fields
    
    private readonly List<Guid> _studentIds = [];
    
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
    
    public IReadOnlyCollection<Guid> StudentIds => _studentIds.AsReadOnly();

    /// <summary>
    /// Gets or sets the date and time when the group was created in UTC.
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the group was last modified in UTC.
    /// </summary>
    public DateTime? ModifiedOnUtc { get; set; }

    #endregion
    
    #region Own methods

    public Result UpdateName(GroupName newName)
    {
        #region Update fields
        
        Name = newName;
        
        #endregion
        
        return Result.Success();
    }

    public Result AddStudent(Guid studentId)
    {
        #region Add student id to group
        
        _studentIds.Add(studentId);
        
        #endregion
        
        return Result.Success();
    }
    
    #endregion
}