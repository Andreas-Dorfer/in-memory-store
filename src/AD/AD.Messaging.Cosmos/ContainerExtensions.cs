using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
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

        public static async Task<TEntity> ReadEntityAsync<TEntity>(this Container container, string id)
            where TEntity : Entity
        {
            var response = await container.ReadItemAsync<TEntity>(id, new PartitionKey(id));
            return response.Resource;
        }

        public static void GetEntityChangeFeedProcessorBuilder<TEntity, TMessage>(this Container container, string processorName, ChangesHandler<TEntity>? onEntityChangesDelegate = null, ChangesHandler<TMessage>? onMessageChangesDelegate = null)
            where TEntity : Entity
            where TMessage : Message
        {
            static string? GetEntityType(dynamic item) => item._entityType;

            container.GetChangeFeedProcessorBuilder<dynamic>(processorName, async (changes, cancellationToken) =>
            {
                if (changes.Count == 0) return;

                var typedChanges = from typedChange in
                                       from change in changes select (Change: change, EntityType: GetEntityType(change))
                                   where typedChange.EntityType is not null
                                   select typedChange;

                if (!typedChanges.Any()) return;

                List<(dynamic Change, string EntityType)> changeBatch = new();

                async Task CallChangesHandler()
                {
                    IReadOnlyCollection<T> CastChangeBatch<T>() => changeBatch.Select(_ => _.Change).Cast<T>().ToList().AsReadOnly();

                    if (changeBatch.Count > 0)
                    {
                        switch (changeBatch[0].EntityType)
                        {
                            case Entity.EntityType:
                                if (onEntityChangesDelegate is not null)
                                {
                                    await onEntityChangesDelegate(CastChangeBatch<TEntity>(), cancellationToken);
                                }
                                break;
                            case Message.EntityType:
                                if (onMessageChangesDelegate is not null)
                                {
                                    await onMessageChangesDelegate(CastChangeBatch<TMessage>(), cancellationToken);
                                }
                                break;
                        }
                        changeBatch.Clear();
                    }
                }

                changeBatch.Add(typedChanges.First());

                foreach (var change in typedChanges.Skip(1))
                {
                    if (change.EntityType != changeBatch[0].EntityType)
                    {
                        await CallChangesHandler();
                    }

                    changeBatch.Add(change);
                }

                if (changeBatch.Count > 0)
                {
                    await CallChangesHandler();
                }
            });
        }
    }
}
