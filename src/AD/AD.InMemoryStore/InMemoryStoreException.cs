using System;

namespace AD.InMemoryStore
{
    /// <summary>
    /// A <see cref="InMemoryStore{TKey, TValue}"/>'s exception.
    /// </summary>
    /// <typeparam name="TKey">The identifying key's type.</typeparam>
    public abstract class InMemoryStoreException<TKey> : Exception
        where TKey : notnull
    {
        internal InMemoryStoreException(TKey key)
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
    public class DuplicateKeyException<TKey> : InMemoryStoreException<TKey>
        where TKey : notnull
    {
        internal DuplicateKeyException(TKey key) : base(key)
        { }
    }

    /// <summary>
    /// The key doesn't exist.
    /// </summary>
    /// <typeparam name="TKey">The identifying key's type.</typeparam>
    public class KeyNotFoundException<TKey> : InMemoryStoreException<TKey>
        where TKey : notnull
    {
        internal KeyNotFoundException(TKey key) : base(key)
        { }
    }

    /// <summary>
    /// Version mismatch.
    /// </summary>
    /// <typeparam name="TKey">The identifying key's type.</typeparam>
    public class ConcurrencyException<TKey> : InMemoryStoreException<TKey>
        where TKey : notnull
    {
        internal ConcurrencyException(TKey key) : base(key)
        { }
    }
}
