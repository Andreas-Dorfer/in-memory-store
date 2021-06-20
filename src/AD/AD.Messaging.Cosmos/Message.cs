using Newtonsoft.Json;

namespace AD.Messaging.Cosmos
{
    public abstract class Message
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("_etag")]
        public string? ETag { get; set; }

        [JsonProperty(Entity.PartitionKey)]
        public string? EntityId { get; set; }

        [JsonProperty(Entity.DiscriminatorKey)]
        public const string EntityType = "message";
    }

    public abstract class DiscriminatedMessage : Message
    {
        [JsonProperty("_messageType")]
        public string? MessageType { get; set; }

        public string? Body { get; set; }
    }
}
