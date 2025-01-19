using InspireEd.Domain.Subjects.Entities;
using InspireEd.Domain.Subjects.ValueObjects;
using InspireEd.Persistence.Subjects.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InspireEd.Persistence.Subjects.Configurations;

/// <summary>
/// Configures the Subject entity for Entity Framework Core.
/// </summary>
internal sealed class SubjectConfiguration : IEntityTypeConfiguration<Subject>
{
    public void Configure(EntityTypeBuilder<Subject> builder)
    {
        // Map to the Subjects table
        builder.ToTable(SubjectTableNames.Subjects);

        // Configure the primary key
        builder.HasKey(x => x.Id);

        // Configure property conversions and constraints
        builder
            .Property(x => x.Name)
            .HasConversion(x => x.Value, v => 
                SubjectName.Create(v).Value)
            .HasMaxLength(SubjectName.MaxLength);

        builder
            .Property(x => x.Code)
            .HasConversion(x => x.Value, v => 
                SubjectCode.Create(v).Value)
            .HasMaxLength(SubjectCode.MaxLength);

        builder
            .Property(x => x.Credit)
            .HasConversion(x => x.Value, v => 
                SubjectCredit.Create(v).Value);

        // Configure CreatedOnUtc and ModifiedOnUtc as required
        builder.Property(x => x.CreatedOnUtc).IsRequired();
        builder.Property(x => x.ModifiedOnUtc).IsRequired(false);
    }
}