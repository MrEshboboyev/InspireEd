using InspireEd.Domain.Primitives;
using InspireEd.Domain.Shared;

namespace InspireEd.Domain.Users.Entities;

public class Teacher : Entity, IAuditableEntity
{
    #region Constructors

    internal Teacher(
        Guid id,
        Guid userId
    ) : base(id)
    {
        UserId = userId;
    }
    
    private Teacher() { }
    
    #endregion
    
    #region Properties
    
    public Guid UserId { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    
    // teacher related fields
    
    #endregion
}