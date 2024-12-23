using InspireEd.Domain.Events;
using InspireEd.Domain.Primitives;
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
        LastName lastName): base(id)
    {
        Email = email;
        PasswordHash = passwordHash;
        FirstName = firstName;
        LastName = lastName;
        LastName = lastName;
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
        LastName lastName
        )
    {
        #region Create new User
        
        var user = new User(
            id,
            email,
            passwordHash,
            firstName,
            lastName);
        
        #endregion

        return user;
    }
    
    #endregion
}

