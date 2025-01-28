using InspireEd.Domain.Classes.Entities;
using InspireEd.Persistence.Classes.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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
        
        builder
            .Property(x => x.GroupIds)
            .HasConversion(
                x => string.Join(",", x), // Convert List<Guid> to string
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(Guid.Parse)
                    .ToList()) // Convert back to List<Guid>
            .Metadata.SetValueComparer(new ValueComparer<IReadOnlyCollection<Guid>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList())); // Ensure EF Core can track changes
        
        // Ignore private fields and configure collections
        builder.Ignore("_attendances");
        builder.Ignore("_groupIds");
    }
}