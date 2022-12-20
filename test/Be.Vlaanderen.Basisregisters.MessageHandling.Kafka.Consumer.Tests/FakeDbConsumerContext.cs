namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer.Tests
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;

    public class FakeDbConsumerContext : ConsumerDbContext<FakeDbConsumerContext>
    {
        private readonly bool _useSameInstance;

        public FakeDbConsumerContext(DbContextOptions<FakeDbConsumerContext> options, bool useSameInstance)
            : base(options)
        {
            _useSameInstance = useSameInstance;
        }

        public override ValueTask DisposeAsync()
        {
            if (_useSameInstance)
            {
                return new ValueTask();
            }

            return base.DisposeAsync();
        }
    }

    public class FakeDbConsumerContextFactory : IDbContextFactory<FakeDbConsumerContext>
    {
        private readonly bool _useSameInstance;
        private readonly string _dbInstance;

        public FakeDbConsumerContextFactory(bool useSameInstance = false)
        {
            _useSameInstance = useSameInstance;
            _dbInstance = Guid.NewGuid().ToString();
        }

        public FakeDbConsumerContext CreateDbContext()
        {
            var builder = new DbContextOptionsBuilder<FakeDbConsumerContext>()
                .UseInMemoryDatabase(_useSameInstance ? _dbInstance : Guid.NewGuid().ToString());

            return new FakeDbConsumerContext(builder.Options, _useSameInstance);
        }
    }
}
