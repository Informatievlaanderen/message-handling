namespace Be.Vlaanderen.Basisregisters.MessageHandling.Kafka.Consumer
{
    using Microsoft.EntityFrameworkCore;

    public interface IOffsetOverrideDbSet
    {
        DbSet<OffsetOverride> OffsetOverrides { get; }
    }
}
