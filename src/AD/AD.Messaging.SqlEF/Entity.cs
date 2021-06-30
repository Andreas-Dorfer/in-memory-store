namespace AD.Messaging.SqlEF
{
    public interface IEntity<TId>
    {
        TId Id { get; }
    }
}
