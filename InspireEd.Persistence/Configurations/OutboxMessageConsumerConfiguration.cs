using InspireEd.Persistence.Constants;
using InspireEd.Persistence.Outbox;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace InspireEd.Persistence.Configurations;

/// <summary> 
/// Configures the OutboxMessageConsumer entity for Entity Framework Core. 
/// </summary>
internal sealed class OutboxMessageConsumerConfiguration : IEntityTypeConfiguration<OutboxMessageConsumer>
{
    public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
    {
        // Map to the OutboxMessageConsumers table
        builder.ToTable(TableNames.OutboxMessageConsumers);

        // Configure the composite primary key
        builder.HasKey(outboxMessageConsumer => new
        {
            outboxMessageConsumer.Id,
            outboxMessageConsumer.Name
        });
    }
}
