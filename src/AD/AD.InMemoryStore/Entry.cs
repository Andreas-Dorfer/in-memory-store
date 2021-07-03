namespace AD.InMemoryStore
{
    class Entry<TValue>
    {
        public static Entry<TValue> Create(TValue value, Version version) => new() { Value = value, Version = version };
        public static Entry<TValue> Compare(Version version) => new() { Version = version };
        public static Entry<TValue> Delete() => new() { Version = Version.Deleted };

        private Entry() { }

        public TValue Value { get; private init; } = default!;
        public Version Version { get; private init; }

        public bool Deleted => Version.Value == Version.Deleted.Value;

        public override bool Equals(object? obj) =>
            !Deleted && obj is Entry<TValue> other && Version.Value == other.Version.Value;

        public override int GetHashCode() =>
            Version.GetHashCode();

        public static implicit operator (TValue, Version)(Entry<TValue> entry) =>
            (entry.Value, entry.Version);
    }
}
