using AD.Messaging.Cosmos;
using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace FTGO.Restaurant.CosmosDbEntities.Core
{
    public class Restaurant : Entity
    {
        public string Name { get; set; } = null!;
    }

    public class RestaurantCreated : Event
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
    }
}
