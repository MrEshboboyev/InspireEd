using InspireEd.Domain.Errors;
using InspireEd.Domain.Primitives;
using InspireEd.Domain.Shared;

namespace InspireEd.Domain.Faculties.ValueObjects;

/// <summary>
/// Represents the name of a group within a faculty.
/// </summary>
public sealed class GroupName : ValueObject
{
    #region Constants

    /// <summary>
    /// Maximum length for a group name.
    /// </summary>
    public const int MaxLength = 100;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="GroupName"/> class with the specified value.
    /// </summary>
    /// <param name="value">The value of the group name.</param>
    private GroupName(string value)
    {
        Value = value;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the value of the group name.
    /// </summary>
    public string Value { get; }

    #endregion

    #region Factory Methods

    /// <summary>
    /// Creates a <see cref="GroupName"/> instance after validating the input.
    /// </summary>
    /// <param name="name">The string to create the <see cref="GroupName"/> value object from.</param>
    /// <returns>A <see cref="Result{GroupName}"/> object containing the <see cref="GroupName"/> value object or an error.</returns>
    public static Result<GroupName> Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return Result.Failure<GroupName>(DomainErrors.GroupName.Empty);
        }

        if (name.Length > MaxLength)
        {
            return Result.Failure<GroupName>(DomainErrors.GroupName.TooLong);
        }

        return Result.Success(new GroupName(name));
    }

    #endregion

    #region Overrides

    /// <summary>
    /// Returns the atomic values of the <see cref="GroupName"/> object for equality checks.
    /// </summary>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    #endregion
}