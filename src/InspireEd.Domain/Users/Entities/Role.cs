using InspireEd.Domain.Primitives;

namespace InspireEd.Domain.Users.Entities;

/// <summary> 
/// Represents a user role in the system. 
/// </summary>
public sealed class Role(
    int id,
    string name) : Enumeration<Role>(id, name)
{
    public static readonly Role Admin = new(1, "Admin");
    public static readonly Role DepartmentHead = new(2, "DepartmentHead");
    public static readonly Role Teacher = new(3, "Teacher");
    public static readonly Role Student = new(4, "Student");

    public ICollection<Permission> Permissions { get; set; }
    public ICollection<User> Users { get; set; }
}