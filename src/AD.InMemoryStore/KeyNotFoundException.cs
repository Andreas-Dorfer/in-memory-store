namespace AD.InMemoryStore
{
    /// <summary>
    /// The key doesn't exist.
    /// </summary>
    /// <typeparam name="TKey">The identifying key's type.</typeparam>
    public class KeyNotFoundException<TKey> : KeyValueStoreException<TKey>
        where TKey : notnull
    {
        internal KeyNotFoundException(TKey key) : base(key)
        { }
    }
}
