namespace AD.InMemoryStore.Functional.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open AD.InMemoryStore.Functional

[<TestClass>]
type TestClass () =

    let failOkExpected () = Assert.Fail "Ok expected"
    let failErrorExpected () = Assert.Fail "Error expected"

    [<TestMethod>]
    member _.``Ok Add`` () =
        let sut = InMemoryStore<_, _> ()
        let expected = "A"

        match sut.Add (Guid.NewGuid(), expected) with
        | Ok (actual, _) -> Assert.AreEqual<_> (expected, actual)
        | Error _ -> failOkExpected ()

    [<TestMethod>]
    member _.``DuplicateKey Error Add`` () =
        let sut = InMemoryStore<_, _> ()
        let key = Guid.NewGuid()
        sut.Add (key, "A") |> ignore
        
        match sut.Add (key, "B") with
        | Error (AddError.DuplicateKey errorKey) -> Assert.AreEqual<_> (key, errorKey)
        | Ok _ -> failErrorExpected ()

    [<TestMethod>]
    member _.``Ok Get`` () =
        let sut = InMemoryStore<_, _> ()
        let key = Guid.NewGuid()
        let expected = sut.Add (key, "A")

        match sut.Get(key) with
        | Ok actual -> Assert.AreEqual<_> (expected, Ok actual)
        | Error _ -> failOkExpected ()

    [<TestMethod>]
    member _.``KeyNotFound Error Get`` () =
        let sut = InMemoryStore<_, _> ()
        let unknownKey = Guid.NewGuid()

        match sut.Get(unknownKey) with
        | Error (GetError.KeyNotFound errorKey) -> Assert.AreEqual<_> (unknownKey, errorKey)
        | Ok _ -> failErrorExpected ()

    [<TestMethod>]
    member _.``GetAll`` () =
        let sut = InMemoryStore<_, _> ()
        let expected =
            [   (Guid.NewGuid(), "A")
                (Guid.NewGuid(), "B")
                (Guid.NewGuid(), "C") ]
            |> List.map (fun (key, value) ->
                match sut.Add (key, value) with
                | Error _ -> raise (AssertFailedException "Add Error")
                | Ok (_, version) -> struct (key, value, version))

        let actual = sut.GetAll ()

        let sortByKey = List.sortBy (fun struct (key, _, _) -> key)
        let expected = expected |> sortByKey
        let actual = actual |> List.ofSeq |> sortByKey
        Assert.AreEqual<_> (expected, actual)
