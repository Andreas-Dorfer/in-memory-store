using Newtonsoft.Json;
using System;

namespace AD.Messaging.Cosmos
{
    public abstract class Entity
    {
        public const string PartitionKey = "_entityId";

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("_etag")]
        public string? ETag { get; set; }

        [JsonProperty(PartitionKey)]
        public Guid EntityId => Id;

        [JsonProperty("_entityType")]
        public const string EntityType = "entity";
    }
}
