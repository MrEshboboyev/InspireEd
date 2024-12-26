using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Repositories;

namespace InspireEd.Domain.Faculties.Repositories;

/// <summary>
/// Defines the repository interface for the Faculty aggregate root.
/// </summary>
public interface IFacultyRepository : IRepository<Faculty>
{
    Task<IEnumerable<Faculty>> GetFacultiesAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Retrieves a faculty entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the faculty.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the faculty entity.</returns>
    Task<Faculty> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new faculty entity to the repository.
    /// </summary>
    /// <param name="faculty">The faculty entity to add.</param>
    void Add(Faculty faculty);

    /// <summary>
    /// Updates an existing faculty entity in the repository.
    /// </summary>
    /// <param name="faculty">The faculty entity to update.</param>
    void Update(Faculty faculty);

    /// <summary>
    /// Removes a faculty entity from the repository.
    /// </summary>
    /// <param name="faculty">The faculty entity to remove.</param>
    void Remove(Faculty faculty);
}