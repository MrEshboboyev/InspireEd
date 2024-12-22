using InspireEd.Domain.Primitives;
using InspireEd.Domain.Shared;

namespace InspireEd.Domain.Users.Entities;

public class DepartmentHead : Entity, IAuditableEntity
{
    #region Constructors

    internal DepartmentHead(
        Guid id,
        Guid userId,
        Guid facultyId
        ) : base(id)
    {
        UserId = userId;
        FacultyId = facultyId;
    }
    
    private DepartmentHead() { }
    
    #endregion
    
    #region Properties
    
    public Guid UserId { get; set; }
    public Guid FacultyId { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ModifiedOnUtc { get; set; }
    
    // DepartmentHead related fields
    
    #endregion
    
    #region Own Methods

    public Result ChangeFaculty(Guid facultyId)
    {
        #region Update Field
        
        FacultyId = facultyId;  
        
        #endregion
        
        return Result.Success();
    }
    
    #endregion
}