namespace AD.InMemoryStore
{
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
    }
}
