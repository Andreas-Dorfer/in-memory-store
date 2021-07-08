namespace AD.InMemoryStore.Functional

type InMemoryStore<'key, 'value> () =
    let store = AD.InMemoryStore.InMemoryStore<'key, 'value> ()

    member _.Add (key, value) =
        try
            let version = store.Add (key, value)
            Ok (key, value, Version version)
        with
        | :? AD.InMemoryStore.DuplicateKeyException<'key> as exn -> Error (AddError.DuplicateKey exn.Key)

    member _.Get key =
        try
            let struct (value, version) = store.Get key
            Ok (key, value, Version version)
        with
        | :? AD.InMemoryStore.KeyNotFoundException<'key> as exn -> Error (GetError.KeyNotFound exn.Key)

    member _.GetAll () =
        store.GetAll ()
        |> Seq.map (fun struct (key, value, version) -> (key, value, Version version))

    member _.Update (key, value, ``match``) =
        try
            let version = store.Update (key, value, ``match`` |> Option.map (fun (Version v) -> v) |> Option.toNullable)
            Ok (key, value, Version version)
        with
        | :? AD.InMemoryStore.KeyNotFoundException<'key> as exn -> Error (UpdateError.KeyNotFound exn.Key)
        | :? AD.InMemoryStore.ConcurrencyException<'key> as exn -> Error (UpdateError.VersionMismatch exn.Key)

    member _.Remove (key, ``match``) =
        try
            Ok (store.Remove (key, ``match`` |> Option.map (fun (Version v) -> v) |> Option.toNullable))
        with
        | :? AD.InMemoryStore.KeyNotFoundException<'key> as exn -> Error (RemoveError.KeyNotFound exn.Key)
        | :? AD.InMemoryStore.ConcurrencyException<'key> as exn -> Error (RemoveError.VersionMismatch exn.Key)
