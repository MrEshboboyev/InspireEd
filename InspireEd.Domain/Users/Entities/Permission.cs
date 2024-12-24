namespace InspireEd.Domain.Users.Entities;

/// <summary> 
/// Represents a role permission in the system. 
/// </summary>
public class Permission
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
}
