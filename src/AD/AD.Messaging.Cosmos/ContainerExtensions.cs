using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace AD.Messaging.Cosmos
{
    public static class ContainerExtensions
    {
        public static async Task<TEntity> CreateEntityAsync<TEntity, TEvent>(this Container container, TEntity entity, TEvent @event)
            where TEntity : Entity
            where TEvent : Event
        {
            var entityId = entity.Id;
            @event.EntityId = entityId;

            var response = await
                container.CreateTransactionalBatch(new(entityId.ToString()))
                .CreateItem(entity)
                .CreateItem(@event)
                .ExecuteAsync();

            return response.GetOperationResultAtIndex<TEntity>(0).Resource;
        }

        public static async Task<TEntity> ReadEntityAsync<TEntity>(this Container container, string id)
            where TEntity : Entity
        {
            var response = await container.ReadItemAsync<TEntity>(id, new(id));
            return response.Resource;
        }
    }
}
