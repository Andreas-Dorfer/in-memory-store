using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace AD.InMemoryStore;

/// <summary>
/// A thread-safe in-memory key-value store with optimistic concurrency support.
/// </summary>
/// <typeparam name="TKey">The identifying key's type.</typeparam>
/// <typeparam name="TValue">The value's type.</typeparam>
public class KeyValueStore<TKey, TValue>
    where TKey : notnull
{
    readonly ConcurrentDictionary<TKey, Entry<TValue>> store = new();

    /// <summary>
    /// Adds a new value.
    /// </summary>
    /// <param name="key">The identifying key.</param>
    /// <param name="value">The value to add.</param>
    /// <returns>The added value's version.</returns>
    /// <exception cref="DuplicateKeyException{TKey}">The key already exists.</exception>
    public Version Add(TKey key, TValue value)
    {
        var entry = Entry<TValue>.Create(value, Version.New);
        return store.TryAdd(key, entry) ? entry.Version : throw new DuplicateKeyException<TKey>(key);
    }

    /// <summary>
    /// Gets a value by key.
    /// </summary>
    /// <param name="key">The identifying key.</param>
    /// <returns>The value and its version.</returns>
    /// <exception cref="KeyNotFoundException{TKey}">The key doesn't exist.</exception>
    public (TValue Value, Version Version) Get(TKey key) =>
        Get(key, out var entry) ? (entry.Value, entry.Version) : throw new KeyNotFoundException<TKey>(key);

    /// <summary>
    /// Gets all values.
    /// </summary>
    /// <returns>All values and their keys and versions.</returns>
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
    /// <returns>The updated value's version.</returns>
    /// <exception cref="KeyNotFoundException{TKey}">The key doesn't exist.</exception>
    /// <exception cref="VersionMismatchException{TKey}">Version mismatch.</exception>
    public Version Update(TKey key, TValue value, Version? match = null)
    {
        if (!GetCompareVersion(key, match, out var compare)) throw new KeyNotFoundException<TKey>(key);
        var next = compare.Next();
        if (!Update(key, Entry<TValue>.Create(value, next), compare)) throw new VersionMismatchException<TKey>(key);
        return next;
    }

    /// <summary>
    /// Removes a value.
    /// </summary>
    /// <param name="key">The identifying key.</param>
    /// <param name="match">The version the value's current version has to match.</param>
    /// <exception cref="KeyNotFoundException{TKey}">The key doesn't exist.</exception>
    /// <exception cref="VersionMismatchException{TKey}">Version mismatch.</exception>
    public void Remove(TKey key, Version? match = null)
    {
        if (!GetCompareVersion(key, match, out var compare)) throw new KeyNotFoundException<TKey>(key);
        if (!Update(key, Entry<TValue>.Delete(), compare)) throw new VersionMismatchException<TKey>(key);
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
