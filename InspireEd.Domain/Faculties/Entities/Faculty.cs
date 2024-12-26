using InspireEd.Domain.Errors;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Domain.Primitives;
using InspireEd.Domain.Shared;

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

    /// <summary>
    /// A list of department head IDs for the faculty.
    /// </summary>
    private readonly List<Guid> _departmentHeadIds = [];

    #endregion

    #region Constructors

    private Faculty(
        Guid id,
        FacultyName name) : base(id)
    {
        Name = name;
    }

    private Faculty() { }

    #endregion

    #region Properties

    public FacultyName Name { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? ModifiedOnUtc { get; set; }

    /// <summary>
    /// Gets the IDs of the department heads assigned to this faculty.
    /// </summary>
    public IReadOnlyCollection<Guid> DepartmentHeadIds => _departmentHeadIds.AsReadOnly();

    #endregion

    #region Factory Methods

    public static Faculty Create(
        Guid id, 
        FacultyName name)
    {
        return new Faculty(id, name);
    }

    #endregion

    #region Methods

    /// <summary>
    /// Assigns a department head to the faculty.
    /// </summary>
    /// <param name="departmentHeadId">The ID of the department head.</param>
    /// <returns>A result indicating success or failure.</returns>
    public Result AddDepartmentHead(Guid departmentHeadId)
    {
        // Check if the department head is already assigned
        if (_departmentHeadIds.Contains(departmentHeadId))
        {
            return Result.Failure(
                DomainErrors.Faculty.DepartmentHeadIdAlreadyExists(departmentHeadId));
        }

        // Add the department head
        _departmentHeadIds.Add(departmentHeadId);

        return Result.Success();
    }

    /// <summary>
    /// Removes a department head from the faculty.
    /// </summary>
    /// <param name="departmentHeadId">The ID of the department head to remove.</param>
    /// <returns>A result indicating success or failure.</returns>
    public Result RemoveDepartmentHead(Guid departmentHeadId)
    {
        if (!_departmentHeadIds.Contains(departmentHeadId))
        {
            return Result.Failure(
                DomainErrors.Faculty.DepartmentHeadIdDoesNotExist(departmentHeadId));
        }

        _departmentHeadIds.Remove(departmentHeadId);

        return Result.Success();
    }

    #endregion
}
