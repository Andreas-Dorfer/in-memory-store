namespace AD.Messaging.SqlEF
{
    public interface IMessage<TMessageId, TEntityId>
    {
        TMessageId Id { get; }
        TEntityId EntityId { get; }
    }
}
