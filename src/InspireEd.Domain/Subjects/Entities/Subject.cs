using InspireEd.Domain.Primitives;
using InspireEd.Domain.Shared;
using InspireEd.Domain.Subjects.ValueObjects;

namespace InspireEd.Domain.Subjects.Entities;

/// <summary>
/// Represents a subject entity.
/// </summary>
public sealed class Subject : AggregateRoot, IAuditableEntity
{
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="Subject"/> class with the specified ID, name, code, and credit.
    /// </summary>
    /// <param name="id">The unique identifier of the subject.</param>
    /// <param name="name">The name of the subject.</param>
    /// <param name="code">The code of the subject.</param>
    /// <param name="credit">The credit value of the subject.</param>
    private Subject(
        Guid id,
        SubjectName name,
        SubjectCode code,
        SubjectCredit credit) : base(id)
    {
        Name = name;
        Code = code;
        Credit = credit;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Subject"/> class.
    /// </summary>
    private Subject()
    {
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets the name of the subject.
    /// </summary>
    public SubjectName Name { get; set; }

    /// <summary>
    /// Gets or sets the code of the subject.
    /// </summary>
    public SubjectCode Code { get; set; }

    /// <summary>
    /// Gets or sets the credit value of the subject.
    /// </summary>
    public SubjectCredit Credit { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the subject was created in UTC.
    /// </summary>
    public DateTime CreatedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the subject was last modified in UTC.
    /// </summary>
    public DateTime? ModifiedOnUtc { get; set; }

    #endregion
    
    #region Factory Methods

    public static Subject Create(
        Guid id,
        SubjectName name,
        SubjectCode code,
        SubjectCredit credit)
    {
        return new Subject(
            id,
            name,
            code,
            credit);
    }
    
    #endregion
    
    #region Own methods
    
    public Result UpdateSubjectDetails(
        SubjectName name,
        SubjectCode code,
        SubjectCredit credit)
    {
        #region Update fields
        
        Name = name;
        Code = code;
        Credit = credit;
        
        #endregion
        
        return Result.Success();
    }
    
    public Result Rename(SubjectName newName)
    {
        #region Update fields
        
        Name = newName;
        
        #endregion
        
        return Result.Success();
    }
    
    public Result ChangeCode(SubjectCode newCode)
    {
        #region Update fields
        
        Code = newCode;
        
        #endregion
        
        return Result.Success();
    }
    
    public Result ChangeCredit(SubjectCredit newCredit)
    {
        #region Update fields
        
        Credit = newCredit;
        
        #endregion
        
        return Result.Success();
    }
    
    #endregion
}