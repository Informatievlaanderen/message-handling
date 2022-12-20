namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer
{
    using Microsoft.EntityFrameworkCore;

    public abstract class ConsumerDbContext<TContext> : DbContext where TContext : DbContext
    {
        public DbSet<ProcessedMessage> ProcessedMessages => Set<ProcessedMessage>();

        protected ConsumerDbContext()
        { }

        protected ConsumerDbContext(DbContextOptions<TContext> dbContextOptions)
            : base(dbContextOptions)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
                OnConfiguringOptionsBuilder(optionsBuilder);
        }

        protected virtual void OnConfiguringOptionsBuilder(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<ProcessedMessage>()
                .HasKey(p => p.IdempotenceKey);

            modelBuilder
                .Entity<ProcessedMessage>()
                .Property(p => p.IdempotenceKey)
                .HasMaxLength(128); //SHA512 length

            modelBuilder
                .Entity<ProcessedMessage>()
                .Property(p => p.DateProcessed);
        }
    }
}
