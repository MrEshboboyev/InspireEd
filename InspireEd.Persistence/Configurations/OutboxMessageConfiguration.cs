﻿using InspireEd.Persistence.Constants;
using InspireEd.Persistence.Outbox;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InspireEd.Persistence.Configurations;

/// <summary> 
/// Configures the OutboxMessage entity for Entity Framework Core. 
/// </summary>
internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        // Map to the OutboxMessages table
        builder.ToTable(TableNames.OutboxMessages);

        // Configure the primary key
        builder.HasKey(x => x.Id);
    }
}
