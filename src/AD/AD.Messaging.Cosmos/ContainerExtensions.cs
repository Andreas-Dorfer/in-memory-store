using Microsoft.Azure.Cosmos;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using static Microsoft.Azure.Cosmos.Container;

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
                container.CreateTransactionalBatch(new PartitionKey(entityId))
                .CreateItem(entity)
                .CreateItem(message)
                .ExecuteAsync();

            return response.GetOperationResultAtIndex<TEntity>(0).Resource;
        }

        public static async Task<TEntity?> ReadEntityAsync<TEntity>(this Container container, string id)
            where TEntity : Entity
        {
            try
            {
                var response = await container.ReadItemAsync<TEntity>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public static ChangeFeedProcessorBuilder GetMessageChangeFeedProcessorBuilder<TMessage>(this Container container, string processorName, ChangesHandler<TMessage> onMessageChangesDelegate)
            where TMessage : Message =>
            container.GetChangeFeedProcessorBuilder<dynamic>(processorName, async (changes, cancellationToken) =>
            {
                var messages = from change in changes
                               where change._entityType == Message.EntityType
                               select ((JObject)change).ToObject<TMessage>();

                await onMessageChangesDelegate(messages.ToList().AsReadOnly(), cancellationToken);
            });
    }
}
