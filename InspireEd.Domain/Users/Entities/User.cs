﻿using InspireEd.Domain.Primitives;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Users.Events;
using InspireEd.Domain.Users.ValueObjects;

namespace InspireEd.Domain.Users.Entities;

/// <summary>
/// Represents a user in the system.
/// </summary>
public sealed class User : AggregateRoot, IAuditableEntity
{
    #region Constructors

    private User(
        Guid id,
        Email email,
        string passwordHash,
        FirstName firstName,
        LastName lastName) : base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
        Roles = [];

        #region Domain Events

        RaiseDomainEvent(new UserCreatedDomainEvent(
            Guid.NewGuid(),
            id,
            email.Value));

        #endregion
    }

    private User()
    {
    }

    #endregion

    #region Properties

    public FirstName FirstName { get; private set; }
    public LastName LastName { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    public ICollection<Role> Roles { get; private set; }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Creates a new user instance.
    /// </summary>
    public static User Create(
        Guid id,
        Email email,
        string passwordHash,
        FirstName firstName,
        LastName lastName,
        Role role)
    {
        var user = new User(id, email, passwordHash, firstName, lastName);
        user.AddRole(role);
        return user;
    }

    #endregion

    #region Methods

    public void AddRole(Role role)
    {
        ArgumentNullException.ThrowIfNull(role);
        if (!Roles.Contains(role))
            Roles.Add(role);
    }

    public void RemoveRole(Role role)
    {
        ArgumentNullException.ThrowIfNull(role);
        Roles.Remove(role);
    }

    /// <summary>
    /// Updates the details of the user.
    /// </summary>
    /// <param name="firstName">The new first name of the user.</param>
    /// <param name="lastName">The new last name of the user.</param>
    /// <param name="email">The new email of the user.</param>
    public Result UpdateDetails(FirstName firstName, LastName lastName, Email email)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;

        // RaiseDomainEvent(new UserUpdatedDomainEvent(
        //     Id,
        //     firstName.Value,
        //     lastName.Value,
        //     email.Value));

        return Result.Success();
    }

    #endregion
}