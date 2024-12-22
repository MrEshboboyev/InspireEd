using InspireEd.Domain.Primitives;

namespace InspireEd.Domain.Users.Entities;

public class Admin : Entity, IAuditableEntity
{
    #region Constructors

    internal Admin(
        Guid id,
        Guid userId
        ) : base(id)
    {
        UserId = userId;
    }
    
    private Admin() { }
    
    #endregion
    
    #region Properties
    
    public Guid UserId { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    
    // Admin related fields
    
    #endregion
}