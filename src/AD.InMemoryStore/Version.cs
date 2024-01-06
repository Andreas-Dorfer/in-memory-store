namespace AD.InMemoryStore;

/// <summary>
/// A value's version.
/// </summary>
public readonly struct Version
{
    internal static readonly Version Deleted = new(0);
    internal static readonly Version New = new(1);

    internal Version(ulong Value)
    {
        this.Value = Value;
    }

    internal readonly ulong Value { get; }

    internal readonly Version Next()
    {
        var next = unchecked(Value + 1);
        return new(next > 0 ? next : 1);
    }

    static readonly Range StringVersionRange = new(1, new Index(1, fromEnd: true));

    /// <summary>
    /// Returns a textual representation.
    /// </summary>
    /// <returns>The version's textual representation.</returns>
    public override string ToString() =>
        '"' + Value.ToString() + '"';

    /// <summary>
    /// Converts a textual representation to a Version.
    /// </summary>
    /// <param name="text">The textual representation.</param>
    /// <returns>The version.</returns>
    /// <exception cref="ArgumentException">Invalid version.</exception>
    public static Version Parse(string text) =>
        TryParse(text, out var version) ? version : throw new ArgumentException("Invalid version.", nameof(text));

    /// <summary>
    /// Converts a textual representation to a Version.
    /// </summary>
    /// <param name="text">The textual representation.</param>
    /// <param name="version">The version.</param>
    /// <returns>True if the conversion was successful, otherwise false.</returns>
    public static bool TryParse(string text, out Version version)
    {
        if (text is null ||
            text.Length < 3 ||
            !ulong.TryParse(text.AsSpan(StringVersionRange), out var value))
        {
            version = default;
            return false;
        }

        version = new(value);
        return true;
    }
}
