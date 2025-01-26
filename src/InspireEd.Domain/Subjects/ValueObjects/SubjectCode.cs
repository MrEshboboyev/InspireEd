using InspireEd.Domain.Errors;
using InspireEd.Domain.Primitives;
using InspireEd.Domain.Shared;

namespace InspireEd.Domain.Subjects.ValueObjects;

/// <summary>
/// Represents the code of a subject.
/// </summary>
public sealed class SubjectCode : ValueObject
{
    #region Constants

    /// <summary>
    /// Maximum length for a subject code.
    /// </summary>
    public const int MaxLength = 10;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectCode"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value of the subject code.</param>
    private SubjectCode(string value)
    {
        Value = value;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the value of the subject code.
    /// </summary>
    public string Value { get; }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Creates a <see cref="SubjectCode"/> instance after validating the input.
    /// </summary>
    /// <param name="code">The string to create the <see cref="SubjectCode"/> value object from.</param>
    /// <returns>A <see cref="Result{SubjectCode}"/> object containing the <see cref="SubjectCode"/> value object or an error.</returns>
    public static Result<SubjectCode> Create(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            return Result.Failure<SubjectCode>(DomainErrors.SubjectCode.Empty);
        }

        if (code.Length > MaxLength)
        {
            return Result.Failure<SubjectCode>(DomainErrors.SubjectCode.TooLong);
        }

        return Result.Success(new SubjectCode(code));
    }

    #endregion

    #region Overrides

    /// <summary>
    /// Returns the atomic values of the <see cref="SubjectCode"/> object for equality checks.
    /// </summary>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    #endregion
}