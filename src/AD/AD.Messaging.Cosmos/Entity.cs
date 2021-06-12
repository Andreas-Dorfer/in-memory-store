using Newtonsoft.Json;

namespace AD.Messaging.Cosmos
{
    public abstract class Entity
    {
        public const string PartitionKey = "_entityId";
        public const string DiscriminatorKey = "_entityType";

        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("_etag")]
        public string? ETag { get; set; }

        [JsonProperty(PartitionKey)]
        public string? EntityId => Id;

        [JsonProperty(DiscriminatorKey)]
        public const string EntityType = "entity";
    }
}
