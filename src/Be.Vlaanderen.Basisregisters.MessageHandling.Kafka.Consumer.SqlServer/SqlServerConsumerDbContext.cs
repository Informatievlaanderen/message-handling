namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer.SqlServer
{
    using Microsoft.EntityFrameworkCore;

    public abstract class SqlServerConsumerDbContext<TContext> : ConsumerDbContext<TContext> where TContext : DbContext
    {
        public const string ProcessedMessageTable = "ProcessedMessages";

        public abstract string ProcessedMessagesSchema { get; }

        protected SqlServerConsumerDbContext()
        { }

        protected SqlServerConsumerDbContext(DbContextOptions<TContext> dbContextOptions)
            : base(dbContextOptions)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
                OnConfiguringOptionsBuilder(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<ProcessedMessage>()
                .ToTable(ProcessedMessageTable, ProcessedMessagesSchema)
                .HasKey(p => p.IdempotenceKey)
                .IsClustered();

            modelBuilder
                .Entity<ProcessedMessage>()
                .Property(p => p.IdempotenceKey)
                .HasMaxLength(128); //SHA512 length

            modelBuilder
                .Entity<ProcessedMessage>()
                .Property(p => p.DateProcessed);

            modelBuilder.ApplyConfiguration(new ConsumerStatesConfiguration(ProcessedMessagesSchema));
        }
    }
}
