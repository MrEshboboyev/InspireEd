using InspireEd.Domain.Errors;
using InspireEd.Domain.Primitives;
using InspireEd.Domain.Shared;

namespace InspireEd.Domain.Subjects.ValueObjects;

/// <summary>
/// Represents the credit value of a subject.
/// </summary>
public sealed class SubjectCredit : ValueObject
{
    #region Constants

    /// <summary>
    /// Maximum value for subject credit.
    /// </summary>
    public const int MaxCredit = 8;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectCredit"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value of the subject credit.</param>
    private SubjectCredit(int value)
    {
        Value = value;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the value of the subject credit.
    /// </summary>
    public int Value { get; }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Creates a <see cref="SubjectCredit"/> instance after validating the input.
    /// </summary>
    /// <param name="credit">The integer value to create the <see cref="SubjectCredit"/> value object from.</param>
    /// <returns>A <see cref="Result{SubjectCredit}"/> object containing the <see cref="SubjectCredit"/> value object
    /// or an error.</returns>
    public static Result<SubjectCredit> Create(int credit)
    {
        return credit switch
        {
            <= 0 => Result.Failure<SubjectCredit>(DomainErrors.SubjectCredit.InvalidCredit),
            > MaxCredit => Result.Failure<SubjectCredit>(DomainErrors.SubjectCredit.TooHigh),
            _ => Result.Success(new SubjectCredit(credit))
        };
    }

    #endregion

    #region Overrides

    /// <summary>
    /// Returns the atomic values of the <see cref="SubjectCredit"/> object for equality checks.
    /// </summary>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    #endregion
}