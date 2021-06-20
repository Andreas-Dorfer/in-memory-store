using AD.Messaging.Cosmos;
using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FTGO.Restaurant.CosmosDbEntities.Core
{
    public class Restaurant : Entity
    {
        public string Name { get; set; } = null!;
        public List<MenuItem> Menu { get; set; } = new();
    }

    public record MenuItem(string Id, string Name, decimal Price);

    public class RestaurantCreated : Message
    {
        public string Name { get; set; } = null!;
    }

    public class RestaurantDataAccess
    {
        readonly Container container;

        public RestaurantDataAccess(Container container)
        {
            this.container = container;
        }

        public async Task<Restaurant> Create(Restaurant entity, RestaurantCreated @event) =>
            await container.CreateEntityAsync(entity, @event);

        public async Task<Restaurant?> Read(Guid id) =>
            await container.ReadEntityAsync<Restaurant>(id.ToString());

        public ChangeFeedProcessor CreateMessageFeedProcessor(Container leaseContainer, Func<RestaurantCreated, Task> onNewMessage)
        {
            var processor = container.GetMessageChangeFeedProcessorBuilder<RestaurantCreated>(nameof(Restaurant) + "MessageProcessor", async (messages, cancellationToken) =>
              {
                  foreach (var message in messages)
                  {
                      await onNewMessage(message);
                      cancellationToken.ThrowIfCancellationRequested();
                  }
              }).WithLeaseContainer(leaseContainer)
                .WithInstanceName("processor1")
                .Build();

            return processor;
        }
    }
}
