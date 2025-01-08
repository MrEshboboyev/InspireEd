using InspireEd.Domain.Faculties.Entities;
using InspireEd.Domain.Faculties.ValueObjects;
using InspireEd.Persistence.Faculties.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InspireEd.Persistence.Faculties.Groups.Configurations;

/// <summary>
/// Configures the Group entity for Entity Framework Core.
/// </summary>
internal sealed class GroupConfiguration : IEntityTypeConfiguration<Group>
{
    public void Configure(EntityTypeBuilder<Group> builder)
    {
        // Map to the Groups table
        builder.ToTable(FacultyTableNames.Groups);

        // Configure the primary key
        builder.HasKey(x => x.Id);

        // Configure foreign key relationship with Faculty
        builder
            .Property(x => x.FacultyId)
            .IsRequired();

        builder
            .HasOne<Faculty>()
            .WithMany()
            .HasForeignKey(x => x.FacultyId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure property conversions and constraints
        builder
            .Property(x => x.Name)
            .HasConversion(x => x.Value, v => GroupName.Create(v).Value)
            .HasMaxLength(GroupName.MaxLength);

        // Configure CreatedOnUtc and ModifiedOnUtc as required
        builder.Property(x => x.CreatedOnUtc).IsRequired();
        builder.Property(x => x.ModifiedOnUtc).IsRequired(false);

        // Configure StudentIds as a value collection
        builder
            .Property(x => x.StudentIds)
            .HasConversion(
                x => string.Join(",", x),  // Convert collection to string for storage
                v => v.Split(',', 
                    StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse).ToList()) // Convert back to collection
            .IsRequired();

        // Ignore private fields
        builder.Ignore("_studentIds");
    }
}