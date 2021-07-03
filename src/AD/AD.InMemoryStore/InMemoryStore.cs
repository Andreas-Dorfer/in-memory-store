using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace AD.InMemoryStore
{
    /// <summary>
    /// A thread-safe in-memory store with versioning.
    /// </summary>
    /// <typeparam name="TKey">The identifying key's type.</typeparam>
    /// <typeparam name="TValue">The value's type.</typeparam>
    public class InMemoryStore<TKey, TValue>
        where TKey : notnull
    {
        readonly ConcurrentDictionary<TKey, Entry<TValue>> store = new();

        /// <summary>
        /// Adds a new value.
        /// </summary>
        /// <param name="key">The identifying key.</param>
        /// <param name="value">The value to add.</param>
        /// <returns>The added value and its version.</returns>
        /// <exception cref="DuplicateKeyException{TKey}">The key already exists.</exception>
        public (TValue Value, Version Version) Add(TKey key, TValue value)
        {
            var entry = Entry<TValue>.Create(value, Version.New);
            return store.TryAdd(key, entry) ? entry : throw new DuplicateKeyException<TKey>(key);
        }

        /// <summary>
        /// Gets a value by key.
        /// </summary>
        /// <param name="key">The identifying key.</param>
        /// <returns>The value and its version.</returns>
        /// <exception cref="KeyNotFoundException{TKey}">The key doesn't exist.</exception>
        public (TValue Value, Version Version) Get(TKey key) =>
            Get(key, out var entry) ? entry : throw new KeyNotFoundException<TKey>(key);

        /// <summary>
        /// Gets all values.
        /// </summary>
        /// <returns>All values and their versions.</returns>
        public IEnumerable<(TKey Key, TValue Value, Version Version)> GetAll() =>
            from keyValue in store
            let entry = keyValue.Value
            where !entry.Deleted
            select (keyValue.Key, entry.Value, entry.Version);

        /// <summary>
        /// Updates a value.
        /// </summary>
        /// <param name="key">The identifying key.</param>
        /// <param name="value">The updated value.</param>
        /// <param name="match">The version the value's current version has to match.</param>
        /// <returns>The updated value and its version.</returns>
        /// <exception cref="KeyNotFoundException{TKey}">The key doesn't exist.</exception>
        /// <exception cref="ConcurrencyException{TKey}">Version mismatch.</exception>
        public (TValue Value, Version Version) Update(TKey key, TValue value, Version? match = null)
        {
            if (!GetCompareVersion(key, match, out var compare)) throw new KeyNotFoundException<TKey>(key);
            var next = compare.Next();
            if (!Update(key, Entry<TValue>.Create(value, next), compare)) throw new ConcurrencyException<TKey>(key);
            return (value, next);
        }

        /// <summary>
        /// Removes a value.
        /// </summary>
        /// <param name="key">The identifying key.</param>
        /// <param name="match">The version the value's current version has to match.</param>
        /// <exception cref="KeyNotFoundException{TKey}">The key doesn't exist.</exception>
        /// <exception cref="ConcurrencyException{TKey}">Version mismatch.</exception>
        public void Remove(TKey key, Version? match = null)
        {
            if (!GetCompareVersion(key, match, out var compare)) throw new KeyNotFoundException<TKey>(key);
            if (!Update(key, Entry<TValue>.Delete(), compare)) throw new ConcurrencyException<TKey>(key);
            store.TryRemove(key, out _);
        }


        bool Get(TKey key, [MaybeNullWhen(false)] out Entry<TValue> entry) =>
            store.TryGetValue(key, out entry) && !entry.Deleted;

        bool Update(TKey key, Entry<TValue> newEntry, Version compare) =>
            store.TryUpdate(key, newEntry, Entry<TValue>.Compare(compare));

        bool GetCurrentVersion(TKey key, out Version version)
        {
            if (!Get(key, out var entry))
            {
                version = default;
                return false;
            }
            version = entry.Version;
            return true;
        }

        bool GetCompareVersion(TKey key, Version? version, out Version compare)
        {
            if (version is null)
            {
                if (!GetCurrentVersion(key, out compare)) return false;
            }
            else
            {
                compare = version.Value;
            }
            return true;
        }
    }
}
