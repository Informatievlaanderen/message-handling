namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public abstract class ConsumerDbContext<TContext> : DbContext where TContext : DbContext
    {
        public DbSet<ProcessedMessage> ProcessedMessages => Set<ProcessedMessage>();
        public DbSet<ConsumerStateItem> ConsumerStates => Set<ConsumerStateItem>();

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

            modelBuilder.ApplyConfiguration(new ConsumerStatesConfiguration());
        }

        public async Task UpdateConsumerState(ConsumerName consumerName, Offset offset, CancellationToken cancellationToken)
        {
            var state = await ConsumerStates
                .FindAsync(new object[] { consumerName.ToString() }, cancellationToken);

            if (state is null)
            {
                await ConsumerStates
                    .AddAsync(new ConsumerStateItem
                    {
                        Name = consumerName.ToString(),
                        Offset = offset
                    }, cancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                state.Offset = offset;
            }
        }

        public async Task Error(ConsumerName consumerName, string errorMessage, CancellationToken cancellationToken)
        {
            var state = await ConsumerStates
                .FindAsync(new object[] { consumerName.ToString() }, cancellationToken);

            if (state is null)
            {
                await ConsumerStates
                    .AddAsync(new ConsumerStateItem
                    {
                        Name = consumerName.ToString(),
                        ErrorMessage = errorMessage
                    }, cancellationToken)
                    .ConfigureAwait(false);
            }
            else
            {
                state.ErrorMessage = errorMessage;
            }
        }
    }
}
