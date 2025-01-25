using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Persistence.Faculties.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InspireEd.Persistence.Faculties.Configurations;

/// <summary>
/// Configures the Faculty entity for Entity Framework Core.
/// </summary>
internal sealed class FacultyConfiguration : IEntityTypeConfiguration<Faculty>
{
    public void Configure(EntityTypeBuilder<Faculty> builder)
    {
        // Map to the Faculties table
        builder.ToTable(FacultyTableNames.Faculties);

        // Configure the primary key
        builder.HasKey(x => x.Id);

        // Configure property conversions and constraints
        builder
            .Property(x => x.Name)
            .HasConversion(x => x.Value, v => 
                FacultyName.Create(v).Value)
            .HasMaxLength(FacultyName.MaxLength);

        // Configure CreatedOnUtc and ModifiedOnUtc as required
        builder.Property(x => x.CreatedOnUtc).IsRequired();
        builder.Property(x => x.ModifiedOnUtc).IsRequired(false);

        builder
            .Property(x => x.DepartmentHeadIds)
            .HasConversion(
                x => string.Join(",", x), // Convert List<Guid> to string
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse)
                    .ToList()) // Convert back to List<Guid>
            .Metadata.SetValueComparer(new ValueComparer<IReadOnlyCollection<Guid>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList())); // Ensure EF Core can track changes

        // Ignore private fields
        builder.Ignore("_groups");
        builder.Ignore("_departmentHeadIds");
    }
}