namespace AD.InMemoryStore.Functional

type AddError<'key> = DuplicateKey of 'key

type GetError<'key> = KeyNotFound of 'key

type UpdateError<'key> =
    | KeyNotFound of 'key
    | VersionMismatch of 'key

type RemoveError<'key> =
    | KeyNotFound of 'key
    | VersionMismatch of 'key
