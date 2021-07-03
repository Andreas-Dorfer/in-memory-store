namespace AD.InMemoryStore.Functional

open AD.InMemoryStore

type AddError<'key> = DuplicateKey of 'key
type GetError<'key> = KeyNotFound of 'key
type UpdateError<'key> =
    | KeyNotFound of 'key
    | VersionMismatch of 'key
type RemoveError<'key> =
    | KeyNotFound of 'key
    | VersionMismatch of 'key

type InMemoryStore<'key, 'value> () =
    let store = AD.InMemoryStore.InMemoryStore<'key, 'value> ()

    member _.Add (key, value) =
        try
            Ok (store.Add (key, value))
        with
        | :? DuplicateKeyException<'key> as ex -> Error (AddError<_>.DuplicateKey key)

    member _.Get key =
        try
            Ok (store.Get key)
        with
        | :? KeyNotFoundException<'key> as ex -> Error (GetError<_>.KeyNotFound key)

    member _.GetAll () =
        store.GetAll ()

    member _.Update (key, value, ``match``) =
        try
            Ok (store.Update (key, value, ``match`` |> Option.toNullable))
        with
        | :? KeyNotFoundException<'key> as ex -> Error (UpdateError<_>.KeyNotFound key)
        | :? ConcurrencyException<'key> as ex -> Error (UpdateError<_>.VersionMismatch key)

    member _.Remove (key, ``match``) =
        try
            Ok (store.Remove (key, ``match`` |> Option.toNullable))
        with
        | :? KeyNotFoundException<'key> as ex -> Error (RemoveError<_>.KeyNotFound key)
        | :? ConcurrencyException<'key> as ex -> Error (RemoveError<_>.VersionMismatch key)
