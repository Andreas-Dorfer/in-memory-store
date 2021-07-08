namespace AD.InMemoryStore.Functional

[<Struct>]
type Version = internal Version of AD.InMemoryStore.Version
    with override this.ToString () = let (Version version) = this in version.ToString()
