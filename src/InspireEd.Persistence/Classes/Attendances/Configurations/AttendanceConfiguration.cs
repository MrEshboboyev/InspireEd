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
        builder.ToTable(ClassTableNames.Attendances);

        builder.HasKey(a => a.Id);

        builder.Property(a => a.StudentId).IsRequired();
        builder.Property(a => a.Status).IsRequired().HasConversion<int>();
        builder.Property(a => a.Notes).HasMaxLength(500);
        builder.Property(a => a.CreatedOnUtc).IsRequired();
        builder.Property(a => a.ModifiedOnUtc).IsRequired(false);

        // ✅ Explicitly map ClassId as a foreign key
        builder.HasOne(a => a.Class)
            .WithMany(c => c.Attendances)
            .HasForeignKey(a => a.ClassId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}