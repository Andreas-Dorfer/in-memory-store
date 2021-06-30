using Microsoft.EntityFrameworkCore;

namespace AD.Messaging.SqlEF
{
    public interface IOutboxDbContext<TMessage> where TMessage : class
    {
        DbSet<TMessage> Outbox { get; }
    }

    public static class OutboxDbContextExtensions
    {
        public static void Publish<TDbContext, TOutMessage>(this TDbContext dbContext, TOutMessage message)
            where TDbContext : DbContext, IOutboxDbContext<TOutMessage>
            where TOutMessage : class
        {
            dbContext.Outbox.Add(message);
        }
    }
}
