using InspireEd.Domain.Classes.Entities;
using InspireEd.Persistence.Classes.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InspireEd.Persistence.Classes.Attendances.Configurations;

/// <summary>
/// EF configuration for the Attendance entity.
/// </summary>
public class AttendanceConfiguration : IEntityTypeConfiguration<Attendance>
{
    public void Configure(EntityTypeBuilder<Attendance> builder)
    {
        // Table name
        builder.ToTable(ClassTableNames.Attendances);

        // Primary key
        builder.HasKey(a => a.Id);

        // Property configurations
        builder.Property(a => a.StudentId).IsRequired();
        builder.HasOne<Class>()
            .WithMany()
            .HasForeignKey(a => a.ClassId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(a => a.Status).IsRequired().HasConversion<int>(); // Enum stored as int
        builder.Property(a => a.Notes).HasMaxLength(500);
        builder.Property(a => a.CreatedOnUtc).IsRequired();
        builder.Property(a => a.ModifiedOnUtc).IsRequired(false);

        // Indexes
        builder.HasIndex(a => new { a.ClassId, a.StudentId }).IsUnique();
    }
}