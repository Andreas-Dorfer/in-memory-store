using Newtonsoft.Json;

namespace AD.Messaging.Cosmos
{
    public abstract class Entity
    {
        public const string PartitionKey = "_entityId";

        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("_etag")]
        public string? ETag { get; set; }

        [JsonProperty(PartitionKey)]
        public string? EntityId => Id;

        [JsonProperty("_entityType")]
        public const string EntityType = "entity";
    }
}
