namespace AD.InMemoryStore.Functional

open AD.InMemoryStore

type InMemoryStore<'key, 'value> () =
    let store = AD.InMemoryStore.InMemoryStore<'key, 'value> ()

    member _.Add (key, value) =
        try
            Ok (store.Add (key, value))
        with
        | :? DuplicateKeyException<'key> as ex -> Error (AddError<_>.DuplicateKey ex.Key)

    member _.Get key =
        try
            Ok (store.Get key)
        with
        | :? KeyNotFoundException<'key> as ex -> Error (GetError<_>.KeyNotFound ex.Key)

    member _.GetAll () =
        store.GetAll ()

    member _.Update (key, value, ``match``) =
        try
            Ok (store.Update (key, value, ``match`` |> Option.toNullable))
        with
        | :? KeyNotFoundException<'key> as ex -> Error (UpdateError<_>.KeyNotFound ex.Key)
        | :? ConcurrencyException<'key> as ex -> Error (UpdateError<_>.VersionMismatch ex.Key)

    member _.Remove (key, ``match``) =
        try
            Ok (store.Remove (key, ``match`` |> Option.toNullable))
        with
        | :? KeyNotFoundException<'key> as ex -> Error (RemoveError<_>.KeyNotFound ex.Key)
        | :? ConcurrencyException<'key> as ex -> Error (RemoveError<_>.VersionMismatch ex.Key)
