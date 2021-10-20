namespace AD.InMemoryStore.Functional.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open AD.InMemoryStore.Functional

[<TestClass>]
type VersionTests () =
    
    let store = KeyValueStore ()

    [<TestMethod>]
    member _.``To string and parse round trip`` () =
        match (1, "A") |> store.Add with
        | Error _ -> Assert.Fail "Ok expected"
        | Ok (_, _, expected) ->
            let actual = expected.ToString() |> Version.parse
            Assert.AreEqual<_> (Some expected, actual)
