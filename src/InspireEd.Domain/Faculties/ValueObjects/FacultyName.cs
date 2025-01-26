using InspireEd.Domain.Errors;
using InspireEd.Domain.Primitives;
using InspireEd.Domain.Shared;

namespace InspireEd.Domain.Faculties.ValueObjects;

/// <summary>
/// Represents the name of a faculty.
/// </summary>
public sealed class FacultyName : ValueObject
{
    #region Constants

    /// <summary>
    /// Maximum length for a faculty name.
    /// </summary>
    public const int MaxLength = 100;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="FacultyName"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value of the faculty name.</param>
    private FacultyName(string value)
    {
        Value = value;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the value of the faculty name.
    /// </summary>
    public string Value { get; }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Creates a <see cref="FacultyName"/> instance after validating the input.
    /// </summary>
    /// <param name="name">The string to create the <see cref="FacultyName"/> value object from.</param>
    /// <returns>A <see cref="Result{FacultyName}"/> object containing the <see cref="FacultyName"/> value object or an error.</returns>
    public static Result<FacultyName> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<FacultyName>(DomainErrors.FacultyName.Empty);
        }

        if (name.Length > MaxLength)
        {
            return Result.Failure<FacultyName>(DomainErrors.FacultyName.TooLong);
        }

        return Result.Success(new FacultyName(name));
    }

    #endregion

    #region Overrides

    /// <summary>
    /// Returns the atomic values of the <see cref="FacultyName"/> object for equality checks.
    /// </summary>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    #endregion
}