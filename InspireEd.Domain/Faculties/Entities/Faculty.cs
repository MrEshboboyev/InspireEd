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
    
    #region Own methods

    public Result UpdateName(FacultyName name)
    {
        #region Modified fields
        
        Name = name;
        
        #endregion

        return Result.Success();
    }
    
    #endregion

    #region Department Head Methods

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
    
    #region Group Methods

    /// <summary>
    /// Creates a new group within the faculty.
    /// </summary>
    /// <param name="id">The unique identifier of the group.</param>
    /// <param name="groupName">The name of the group.</param>
    /// <returns>The created group.</returns>
    public Result<Group> AddGroup(
        Guid id,
        GroupName groupName)
    {
        #region Checking group already exists
        
        if (_groups.Any(g => g.Name.Equals(groupName)))
        {
            return Result.Failure<Group>(
                DomainErrors.Faculty.GroupNameAlreadyExists(groupName.Value));
        }
        
        #endregion

        #region Create new group
        
        var group = new Group(
            id, 
            Id, 
            groupName);
        
        #endregion
        
        #region Add group to this faculty
        
        _groups.Add(group);
        
        #endregion
        
        return Result.Success(group);
    }

    /// <summary>
    /// Removes a group from the faculty.
    /// </summary>
    /// <param name="groupId">The ID of the group to remove.</param>
    /// <returns>A result indicating success or failure.</returns>
    public Result RemoveGroup(
        Guid groupId)
    {
        #region Checking group exists
        
        var group = _groups.FirstOrDefault(g => g.Id == groupId);
        if (group is null)
        {
            return Result.Failure(
                DomainErrors.Faculty.GroupDoesNotExist(groupId));
        }
        
        #endregion

        #region Remove group from this faculty
        
        _groups.Remove(group);
        
        #endregion
        
        return Result.Success();
    }

    /// <summary>
    /// Updates the name of a group.
    /// </summary>
    /// <param name="groupId">The ID of the group to update.</param>
    /// <param name="newName">The new name for the group.</param>
    /// <returns>A result indicating success or failure.</returns>
    public Result UpdateGroup(
        Guid groupId,
        GroupName newName)
    {
        #region Checking group exists
        
        var group = _groups.FirstOrDefault(g => g.Id == groupId);
        if (group is null)
        {
            return Result.Failure(
                DomainErrors.Faculty.GroupDoesNotExist(groupId));
        }
        
        #endregion

        #region Checking this group name already exists in this faculty
        
        if (_groups.Any(g => 
                g.Name.Equals(newName) && 
                g.Id != groupId))
        {
            return Result.Failure(
                DomainErrors.Faculty.GroupNameAlreadyExists(newName.Value));
        }
        
        #endregion

        #region Update this group name
        
        var updateGroupNameResult = group.UpdateName(newName);
        if (updateGroupNameResult.IsFailure)
        {
            return Result.Failure(
                updateGroupNameResult.Error);
        }
        
        #endregion
        
        return Result.Success();
    }

    /// <summary>
    /// Retrieves a group by its ID.
    /// </summary>
    /// <param name="groupId">The ID of the group.</param>
    /// <returns>The group if found.</returns>
    public Result<Group> GetGroupById(
        Guid groupId)
    {
        #region Checking group exists
        
        var group = _groups.FirstOrDefault(g => g.Id == groupId);
        if (group is null)
        {
            return Result.Failure<Group>(
                DomainErrors.Faculty.GroupDoesNotExist(groupId));
        }
        
        #endregion

        return Result.Success(group);
    }

    #endregion 
}
