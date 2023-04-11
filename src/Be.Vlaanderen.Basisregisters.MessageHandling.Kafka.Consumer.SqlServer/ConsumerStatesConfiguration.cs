namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer.SqlServer
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public sealed class ConsumerStatesConfiguration : IEntityTypeConfiguration<ConsumerStateItem>
    {
        public const string TableName = "ConsumerStates";

        private readonly string _schema;

        public ConsumerStatesConfiguration(string schema)
        {
            if (string.IsNullOrWhiteSpace(schema))
                throw new ArgumentException("Schema cannot be empty.", nameof(schema));

            _schema = schema;
        }

        public void Configure(EntityTypeBuilder<ConsumerStateItem> b)
        {
            b.ToTable(TableName, _schema)
                .HasKey(x => x.Name)
                .IsClustered();

            b.Property(p => p.Name);
            b.Property(p => p.Offset);
            b.Property(p => p.ErrorMessage);
        }
    }
}
