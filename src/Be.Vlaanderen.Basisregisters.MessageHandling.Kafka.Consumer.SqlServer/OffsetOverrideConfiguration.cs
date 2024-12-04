namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer.SqlServer
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class OffsetOverrideConfiguration : IEntityTypeConfiguration<OffsetOverride>
    {
        public const string TableName = "OffsetOverrides";

        private readonly string _schema;

        public OffsetOverrideConfiguration(string schema)
        {
            _schema = schema;
        }

        public void Configure(EntityTypeBuilder<OffsetOverride> builder)
        {
            builder.ToTable(TableName, _schema)
                .HasKey(x => x.ConsumerGroupId);
        }
    }
}
