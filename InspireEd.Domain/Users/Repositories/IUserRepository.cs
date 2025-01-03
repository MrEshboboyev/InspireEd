﻿using InspireEd.Domain.Users.Entities;
using InspireEd.Domain.Users.ValueObjects;

namespace InspireEd.Domain.Users.Repositories;

/// <summary>
/// Repository interface for managing user data access.
/// </summary>
public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by their unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The user corresponding to the given identifier.</returns>
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<List<User>> GetByIdsAsync(List<Guid> ids, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a user by their email address.
    /// </summary>
    /// <param name="email">The email address of the user.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The user corresponding to the given email address.</returns>
    Task<User> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checks if an email address is unique across all users.
    /// </summary>
    /// <param name="email">The email address to check.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>True if the email is unique; otherwise, false.</returns>
    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of all users.
    /// </summary>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A list of all users.</returns>
    Task<List<User>> GetUsersAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new user to the repository.
    /// </summary>
    /// <param name="user">The user entity to add.</param>
    void Add(User user);

    /// <summary>
    /// Updates an existing user in the repository.
    /// </summary>
    /// <param name="user">The user entity to update.</param>
    void Update(User user);
    
    void Delete(User user);
}