using System;

namespace AD.InMemoryStore
{
    /// <summary>
    /// A <see cref="KeyValueStore{TKey, TValue}"/>'s exception.
    /// </summary>
    /// <typeparam name="TKey">The identifying key's type.</typeparam>
    public abstract class KeyValueStoreException<TKey> : Exception
        where TKey : notnull
    {
        internal KeyValueStoreException(TKey key)
        {
            Key = key;
        }

        /// <summary>
        /// The identifying key.
        /// </summary>
        public TKey Key { get; }
    }

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
}
