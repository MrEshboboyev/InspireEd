using InspireEd.Domain.Users.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission = InspireEd.Domain.Users.Enums.Permission;

namespace InspireEd.Persistence.Users.Configurations.Roles.Permissions;

/// <summary> 
/// Configures the RolePermission entity for Entity Framework Core. 
/// </summary>
internal sealed class RolePermissionConfiguration
    : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        // Configure the composite primary key
        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        // Seed initial data
        builder.HasData(
            Create(Role.Admin, Permission.ReadUser),
            Create(Role.Admin, Permission.UpdateUser),
            Create(Role.DepartmentHead, Permission.AssignGroups),
            Create(Role.DepartmentHead, Permission.AssignClasses),
            Create(Role.Teacher, Permission.ManageAttendance));
    }

    private static RolePermission Create(
        Role role, 
        Permission permission)
    {
        return new RolePermission
        {
            RoleId = role.Id,
            PermissionId = (int)permission
        };
    }
}
