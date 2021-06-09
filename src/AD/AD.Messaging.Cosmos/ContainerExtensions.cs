using Microsoft.Azure.Cosmos;
using System.Threading.Tasks;

namespace AD.Messaging.Cosmos
{
    public static class ContainerExtensions
    {
        public static async Task<TEntity> CreateEntityAsync<TEntity, TMessage>(this Container container, TEntity entity, TMessage message)
            where TEntity : Entity
            where TMessage : Message
        {
            var entityId = entity.Id;
            message.EntityId = entityId;

            var response = await
                container.CreateTransactionalBatch(partitionKey: new(entityId))
                .CreateItem(entity)
                .CreateItem(message)
                .ExecuteAsync();

            return response.GetOperationResultAtIndex<TEntity>(0).Resource;
        }

        public static async Task<TEntity> ReadEntityAsync<TEntity>(this Container container, string id)
            where TEntity : Entity
        {
            var response = await container.ReadItemAsync<TEntity>(id, partitionKey: new(id));
            return response.Resource;
        }
    }
}
