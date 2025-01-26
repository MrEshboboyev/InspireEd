using InspireEd.Domain.Classes.Entities;

namespace InspireEd.Domain.Classes.Repositories;

/// <summary>
/// Defines the repository interface for managing attendance records.
/// </summary>
public interface IAttendanceRepository
{
    Task<Attendance> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Adds a new attendance record to the repository.
    /// </summary>
    /// <param name="attendance">The attendance record to add.</param>
    void Add(Attendance attendance);

    /// <summary>
    /// Updates an existing attendance record in the repository.
    /// </summary>
    /// <param name="attendance">The attendance record to update.</param>
    void Update(Attendance attendance);
}