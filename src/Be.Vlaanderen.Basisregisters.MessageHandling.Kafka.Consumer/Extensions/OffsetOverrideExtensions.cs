namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer.Extensions
{
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public static class OffsetOverrideExtensions
    {
        public static void OverrideConfigureOffset<TDbContext>(this TDbContext dbContext, ConsumerOptions consumerOptions)
            where TDbContext : DbContext, IOffsetOverrideDbSet
        {
            var consumerGroupId = consumerOptions.ConsumerGroupId.ToString();

            var offsetOverride = dbContext.OffsetOverrides
                .SingleOrDefault(x => x.ConsumerGroupId == consumerGroupId && x.Configured == false);

            if (offsetOverride is not null)
            {
                consumerOptions.ConfigureOffset(new Offset(offsetOverride.Offset));
                offsetOverride.Configured = true;
                dbContext.SaveChanges();
            }
        }
    }
}
