using InspireEd.Domain.Faculties.Entities;

namespace InspireEd.Domain.Faculties.Repositories;

/// <summary>
/// Defines the repository interface for the Group entity.
/// </summary>
public interface IGroupRepository
{
    Task<List<Group>> GetByIdsAsync(
        List<Guid> groupIds,
        CancellationToken cancellationToken = default);
    
    Task<Group> GetByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<List<Guid>> GetStudentIdsForGroupsAsync(
        IEnumerable<Guid> groupIds,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new group entity to the repository.
    /// </summary>
    /// <param name="group">The group entity to add.</param>
    void Add(Group group);

    /// <summary>
    /// Updates an existing group entity in the repository.
    /// </summary>
    /// <param name="group">The group entity to update.</param>
    void Update(Group group);

    /// <summary>
    /// Removes a group entity from the repository.
    /// </summary>
    /// <param name="group">The group entity to remove.</param>
    void Remove(Group group);
}