namespace AD.InMemoryStore.Functional

type IErrorKey<'key> =
    abstract member Key: 'key

[<AutoOpen>]
module ErrorKey =
    let (|ErrorKey|) (error: IErrorKey<_>) = error.Key

/// Error adding a new value.
type AddError<'key> =
    /// The key already exists.
    DuplicateKey of 'key
        interface IErrorKey<'key> with
            member this.Key = let (DuplicateKey key) = this in key

/// Error getting a value.
type GetError<'key> =
    /// The key doesn't exist.
    KeyNotFound of 'key
        interface IErrorKey<'key> with
            member this.Key = let (KeyNotFound key) = this in key

/// Error updating a value.
type UpdateError<'key> =
    /// The key doesn't exist.
    | KeyNotFound of 'key
    /// Version mismatch.
    | VersionMismatch of 'key
        interface IErrorKey<'key> with
            member this.Key = let (KeyNotFound key | VersionMismatch key) = this in key

/// Error removing a value.
type RemoveError<'key> =
    /// The key doesn't exist.
    | KeyNotFound of 'key
    /// Version mismatch.
    | VersionMismatch of 'key
        interface IErrorKey<'key> with
            member this.Key = let (KeyNotFound key | VersionMismatch key) = this in key
