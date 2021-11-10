namespace AD.InMemoryStore;

/// <summary>
/// The key already exists.
/// </summary>
/// <typeparam name="TKey">The identifying key's type.</typeparam>
public class DuplicateKeyException<TKey> : KeyValueStoreException<TKey>
    where TKey : notnull
{
    internal DuplicateKeyException(TKey key) : base(key)
    { }
}
