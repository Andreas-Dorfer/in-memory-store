namespace AD.InMemoryStore.Functional

open AD.InMemoryStore

type InMemoryStore<'key, 'value> () =
    let store = AD.InMemoryStore.InMemoryStore<'key, 'value> ()

    member _.Add (key, value) =
        try
            Ok (store.Add (key, value))
        with
        | :? DuplicateKeyException<'key> as exn -> Error (AddError.DuplicateKey exn.Key)

    member _.Get key =
        try
            Ok (store.Get key)
        with
        | :? KeyNotFoundException<'key> as exn -> Error (GetError.KeyNotFound exn.Key)

    member _.GetAll () =
        store.GetAll ()

    member _.Update (key, value, ``match``) =
        try
            Ok (store.Update (key, value, ``match`` |> Option.toNullable))
        with
        | :? KeyNotFoundException<'key> as exn -> Error (UpdateError.KeyNotFound exn.Key)
        | :? ConcurrencyException<'key> as exn -> Error (UpdateError.VersionMismatch exn.Key)

    member _.Remove (key, ``match``) =
        try
            Ok (store.Remove (key, ``match`` |> Option.toNullable))
        with
        | :? KeyNotFoundException<'key> as exn -> Error (RemoveError.KeyNotFound exn.Key)
        | :? ConcurrencyException<'key> as exn -> Error (RemoveError.VersionMismatch exn.Key)
