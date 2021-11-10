namespace AD.InMemoryStore;

/// <summary>
/// Version mismatch.
/// </summary>
/// <typeparam name="TKey">The identifying key's type.</typeparam>
public class VersionMismatchException<TKey> : KeyValueStoreException<TKey>
    where TKey : notnull
{
    internal VersionMismatchException(TKey key) : base(key)
    { }
}
