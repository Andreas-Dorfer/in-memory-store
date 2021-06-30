namespace AD.Messaging.SqlEF
{
    public interface IEntity<TId>
    {
        TId Id { get; }
    }

    public interface IMessage<TMessageId, TEntityId>
    {
        TMessageId Id { get; }
        TEntityId EntityId { get; }
    }
}
