namespace AD.InMemoryStore.Functional

/// Error adding a new value.
type AddError<'key> =
    /// The key already exists.
    DuplicateKey of 'key

/// Error getting a value.
type GetError<'key> =
    /// The key doesn't exist.
    KeyNotFound of 'key

/// Error updating a value.
type UpdateError<'key> =
    /// The key doesn't exist.
    | KeyNotFound of 'key
    /// Version mismatch.
    | VersionMismatch of 'key

/// Error removing a value.
type RemoveError<'key> =
    /// The key doesn't exist.
    | KeyNotFound of 'key
    /// Version mismatch.
    | VersionMismatch of 'key
