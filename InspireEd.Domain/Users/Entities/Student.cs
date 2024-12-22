using InspireEd.Domain.Primitives;
using InspireEd.Domain.Shared;

namespace InspireEd.Domain.Users.Entities;

public sealed class Student : Entity, IAuditableEntity
{
    #region Constructors

    internal Student(
        Guid id,
        Guid userId,
        Guid groupId
        ) : base(id)
    {
        UserId = userId;
        GroupId = groupId;
    }
    
    private Student() { }
    
    #endregion
    
    #region Properties
    
    public Guid UserId { get; set; }
    public Guid GroupId { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    
    #endregion
    
    #region Own Methods

    public Result ChangeGroup(Guid groupId)
    {
        #region Update Field
        
        GroupId = groupId;  
        
        #endregion
        
        return Result.Success();
    }
    
    #endregion
}