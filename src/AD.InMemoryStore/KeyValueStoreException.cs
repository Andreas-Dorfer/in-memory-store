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
}
