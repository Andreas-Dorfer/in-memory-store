using Newtonsoft.Json;
using System;

namespace AD.Messaging.Cosmos
{
    public abstract class Event
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("_etag")]
        public string? ETag { get; set; }

        [JsonProperty("_ts")]
        public DateTime? TimeStamp { get; set; }

        [JsonProperty(Entity.PartitionKey)]
        public Guid EntityId { get; set; }

        [JsonProperty("_entityType")]
        public const string EntityType = "event";
    }
}
