namespace AD.InMemoryStore.Functional

/// A value's version.
[<Struct>]
type Version = internal Version of AD.InMemoryStore.Version
    with override this.ToString () = let (Version version) = this in version.ToString()

/// A value's version.
module Version =

    /// <summary>
    /// Converts a textual representation to a version.
    /// </summary>
    /// <param name="text">The textual representation.</param>
    /// <returns>The version.</returns>
    let parse text =
        match text |> AD.InMemoryStore.Version.TryParse with
        | (true, version) -> Some (Version version)
        | (false, _) -> None
