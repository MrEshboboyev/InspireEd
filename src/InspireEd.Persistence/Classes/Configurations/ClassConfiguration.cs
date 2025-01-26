using InspireEd.Domain.Classes.Entities;
using InspireEd.Persistence.Classes.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InspireEd.Persistence.Classes.Configurations;

/// <summary>
/// EF configuration for the Class entity.
/// </summary>
public class ClassConfiguration : IEntityTypeConfiguration<Class>
{
    public void Configure(EntityTypeBuilder<Class> builder)
    {
        // Table name
        builder.ToTable(ClassTableNames.Classes);

        // Primary key
        builder.HasKey(c => c.Id);

        // Property configurations
        builder.Property(c => c.SubjectId)
            .IsRequired();

        builder.Property(c => c.TeacherId)
            .IsRequired();

        builder.Property(c => c.Type)
            .IsRequired()
            .HasConversion<int>(); // Enum stored as int

        builder.Property(c => c.ScheduledDate)
            .IsRequired();

        // Ignore private fields and configure collections
        builder.Ignore("_attendances");
        builder.Ignore("_groupIds");

        // builder.Metadata.FindNavigation(nameof(Class.Attendances))!
        //     .SetPropertyAccessMode(PropertyAccessMode.Field);
        //
        // builder.Metadata.FindNavigation(nameof(Class.GroupIds))!
        //     .SetPropertyAccessMode(PropertyAccessMode.Field);

        // Relationships
        builder.HasMany<Attendance>()
            .WithOne()
            .HasForeignKey(a => a.ClassId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}