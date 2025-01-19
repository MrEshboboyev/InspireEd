using InspireEd.Domain.Repositories;
using InspireEd.Domain.Subjects.Entities;

namespace InspireEd.Domain.Subjects.Repositories;

/// <summary>
/// Defines the repository interface for the Subject aggregate root.
/// </summary>
public interface ISubjectRepository : IRepository<Subject>
{
    Task<List<Subject>> GetAllAsync(CancellationToken cancellationToken = default);
    
    Task<List<Subject>> GetByCreditRangeAsync(
        int minCredit, 
        int maxCredit,
        CancellationToken cancellationToken = default);
    
    Task<List<Subject>> GetByCreationDateRangeAsync(
        DateTime startDate, 
        DateTime endDate,
        CancellationToken cancellationToken = default);
    
    
    /// <summary>
    /// Retrieves a subject entity by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the subject.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task that represents the asynchronous operation, containing the subject entity.</returns>
    Task<Subject> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new subject entity to the repository.
    /// </summary>
    /// <param name="subject">The subject entity to add.</param>
    void Add(Subject subject);

    /// <summary>
    /// Updates an existing subject entity in the repository.
    /// </summary>
    /// <param name="subject">The subject entity to update.</param>
    void Update(Subject subject);

    /// <summary>
    /// Removes a subject entity from the repository.
    /// </summary>
    /// <param name="subject">The subject entity to remove.</param>
    void Remove(Subject subject);
}