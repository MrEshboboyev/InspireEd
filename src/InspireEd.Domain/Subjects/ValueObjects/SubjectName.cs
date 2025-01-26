using InspireEd.Domain.Errors;
using InspireEd.Domain.Primitives;
using InspireEd.Domain.Shared;

namespace InspireEd.Domain.Subjects.ValueObjects;

/// <summary>
/// Represents the name of a subject.
/// </summary>
public sealed class SubjectName : ValueObject
{
    #region Constants

    /// <summary>
    /// Maximum length for a subject name.
    /// </summary>
    public const int MaxLength = 50;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="SubjectName"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value of the subject name.</param>
    private SubjectName(string value)
    {
        Value = value;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the value of the subject name.
    /// </summary>
    public string Value { get; }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Creates a <see cref="SubjectName"/> instance after validating the input.
    /// </summary>
    /// <param name="name">The string to create the <see cref="SubjectName"/> value object from.</param>
    /// <returns>A <see cref="Result{SubjectName}"/> object containing the <see cref="SubjectName"/> value object or
    /// an error.</returns>
    public static Result<SubjectName> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<SubjectName>(DomainErrors.SubjectName.Empty);
        }

        if (name.Length > MaxLength)
        {
            return Result.Failure<SubjectName>(DomainErrors.SubjectName.TooLong);
        }

        return Result.Success(new SubjectName(name));
    }

    #endregion

    #region Overrides

    /// <summary>
    /// Returns the atomic values of the <see cref="SubjectName"/> object for equality checks.
    /// </summary>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    #endregion
}