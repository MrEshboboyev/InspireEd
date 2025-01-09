using InspireEd.Domain.Classes.Entities;
using InspireEd.Domain.Repositories;

namespace InspireEd.Domain.Classes.Repositories;

/// <summary>
/// Defines the repository interface for the Class aggregate root.
/// </summary>
public interface IClassRepository : IRepository<Class>
{
    /// <summary>
    /// Retrieves a class entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the class.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the class entity.</returns>
    Task<Class> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new class entity to the repository.
    /// </summary>
    /// <param name="classItem">The class entity to add.</param>
    void Add(Class classItem);

    /// <summary>
    /// Updates an existing class entity in the repository.
    /// </summary>
    /// <param name="classItem">The class entity to update.</param>
    void Update(Class classItem);

    /// <summary>
    /// Removes a class entity from the repository.
    /// </summary>
    /// <param name="classItem">The class entity to remove.</param>
    void Remove(Class classItem);

    /// <summary>
    /// Retrieves the IDs of students related to the groups of a specific class.
    /// </summary>
    /// <param name="classId">The unique identifier of the class.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the list of student IDs.</returns>
    Task<List<Guid>> GetGroupStudentIds(Guid classId, CancellationToken cancellationToken = default);
}