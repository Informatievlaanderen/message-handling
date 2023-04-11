namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public sealed class ConsumerStateItem
    {
        public string Name { get; set; }
        public long Offset { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public sealed class ConsumerStatesConfiguration : IEntityTypeConfiguration<ConsumerStateItem>
    {
        public void Configure(EntityTypeBuilder<ConsumerStateItem> b)
        {
            b.HasKey(p => p.Name);
            b.Property(p => p.Name);
            b.Property(p => p.Offset);
            b.Property(p => p.ErrorMessage);
        }
    }
}
