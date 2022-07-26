namespace AD.InMemoryStore.Functional.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open AD.InMemoryStore.Functional

module TestClass =
    
    let failOkExpected () = Assert.Fail "Ok expected"
    let failErrorExpected () = Assert.Fail "Error expected"
    let failNamedErrorExpected name = Assert.Fail (name + " error expected")

    let okValue = function
        | Ok ok -> ok
        | Error _ -> raise (AssertFailedException "Ok expected")

    let assertOkUpdate expectedKey expectedValue initialVersion = function
        | Ok (actualKey, actualValue, actualVersion) ->
            Assert.AreEqual<_> (expectedKey, actualKey)
            Assert.AreEqual<_> (expectedValue, actualValue)
            Assert.AreNotEqual<_> (initialVersion, actualVersion)
        | Error _ -> failOkExpected ()

    let assertOkRemove expectedKey = function
        | Ok actualKey -> Assert.AreEqual<_> (expectedKey, actualKey)
        | Error _ -> failOkExpected ()

open TestClass

[<TestClass>]
type TestClass () =

    [<TestMethod>]
    member _.``Ok Add`` () =
        let sut = KeyValueStore<_, _> ()
        let expectedKey = Guid.NewGuid()
        let expectedValue = "A"

        match sut.Add (expectedKey, expectedValue) with
        | Ok (actualKey, actualValue, _) ->
            Assert.AreEqual<_> (expectedKey, actualKey)
            Assert.AreEqual<_> (expectedValue, actualValue)
        | Error _ -> failOkExpected ()

    [<TestMethod>]
    member _.``DuplicateKey Error Add`` () =
        let sut = KeyValueStore<_, _> ()
        let key = Guid.NewGuid()
        sut.Add (key, "A") |> ignore
        
        match sut.Add (key, "B") with
        | Error (AddError.DuplicateKey errorKey) -> Assert.AreEqual<_> (key, errorKey)
        | Ok _ -> failErrorExpected ()

    [<TestMethod>]
    member _.``Ok Get`` () =
        let sut = KeyValueStore<_, _> ()
        let expectedKey = Guid.NewGuid()
        let expected = sut.Add (expectedKey, "A") |> okValue

        match sut.Get(expectedKey) with
        | Ok actual -> Assert.AreEqual<_> (expected, actual)
        | Error _ -> failOkExpected ()

    [<TestMethod>]
    member _.``KeyNotFound Error Get`` () =
        let sut = KeyValueStore<_, _> ()
        let unknownKey = Guid.NewGuid()

        match sut.Get(unknownKey) with
        | Error (GetError.KeyNotFound errorKey) -> Assert.AreEqual<_> (unknownKey, errorKey)
        | Ok _ -> failErrorExpected ()

    [<TestMethod>]
    member _.``GetAll`` () =
        fun (values : Map<Guid, string>) ->
            let sut = KeyValueStore<_, _> ()
            let expected =
                values
                |> Map.toList
                |> List.map (sut.Add >> okValue)

            let actual = sut.GetAll ()

            let sortByKey = List.sortBy (fun (key, _, _) -> key)
            let expected = expected |> sortByKey
            let actual = actual |> List.ofSeq |> sortByKey
            Assert.AreEqual<_> (expected, actual)
        |> Check.QuickThrowOnFailure

    [<TestMethod>]
    member _.``Ok Update`` () =
        let sut = KeyValueStore<_, _> ()
        let expectedKey = Guid.NewGuid()
        let (_, _, initialVersion) = sut.Add (expectedKey, "A") |> okValue
        let expectedValue = "B"

        sut.Update (expectedKey, expectedValue, Some initialVersion)
        |> assertOkUpdate

    [<TestMethod>]
    member _.``Ok Update with no version check`` () =
        let sut = KeyValueStore<_, _> ()
        let expectedKey = Guid.NewGuid()
        sut.Add (expectedKey, "A") |> ignore
        let expectedValue = "B"

        sut.Update (expectedKey, expectedValue, None)
        |> assertOkUpdate

    [<TestMethod>]
    member _.``KeyNotFound Error Update`` () =
        let sut = KeyValueStore<_, _> ()
        let unknownKey = Guid.NewGuid()
        
        match sut.Update (unknownKey, "X", None) with
        | Error (UpdateError.KeyNotFound errorKey) -> Assert.AreEqual<_> (unknownKey, errorKey)
        | Error _ -> failNamedErrorExpected (nameof UpdateError.KeyNotFound)
        | Ok _ -> failErrorExpected ()

    [<TestMethod>]
    member _.``KeyNotFound Error Update with version check`` () =
        let sut = KeyValueStore<_, _> ()
        let unknownKey = Guid.NewGuid()
        let dummyVersion = "\"1\"" |> Version.parse
        
        match sut.Update (unknownKey, "X", dummyVersion) with
        | Error (UpdateError.KeyNotFound errorKey) -> Assert.AreEqual<_> (unknownKey, errorKey)
        | Error _ -> failNamedErrorExpected (nameof UpdateError.KeyNotFound)
        | Ok _ -> failErrorExpected ()

    [<TestMethod>]
    member _.``VersionMismatch Error Update`` () =
        let sut = KeyValueStore<_, _> ()
        let key = Guid.NewGuid()
        let (_, _, initialVersion) = sut.Add (key, "A") |> okValue
        sut.Update (key, "B", Some initialVersion) |> ignore

        match sut.Update (key, "C", Some initialVersion) with
        | Error (UpdateError.VersionMismatch errorKey) -> Assert.AreEqual<_> (key, errorKey)
        | Error _ -> failNamedErrorExpected (nameof UpdateError.VersionMismatch)
        | Ok _ -> failErrorExpected ()

    [<TestMethod>]
    member _.``Ok Remove`` () =
        let sut = KeyValueStore<_, _> ()
        let expectedKey = Guid.NewGuid()
        let (_, _, initialVersion) = sut.Add (expectedKey, "A") |> okValue

        sut.Remove (expectedKey, Some initialVersion)
        |> assertOkRemove expectedKey

    [<TestMethod>]
    member _.``Ok Remove with no version check`` () =
        let sut = KeyValueStore<_, _> ()
        let expectedKey = Guid.NewGuid()
        sut.Add (expectedKey, "A") |> ignore

        sut.Remove (expectedKey, None)
        |> assertOkRemove expectedKey

    [<TestMethod>]
    member _.``KeyNotFound Error Remove`` () =
        let sut = KeyValueStore<_, _> ()
        let unknownKey = Guid.NewGuid()
        
        match sut.Remove (unknownKey, None) with
        | Error (RemoveError.KeyNotFound errorKey) -> Assert.AreEqual<_> (unknownKey, errorKey)
        | Error _ -> failNamedErrorExpected (nameof RemoveError.KeyNotFound)
        | Ok _ -> failErrorExpected ()

    [<TestMethod>]
    member _.``KeyNotFound Error Remove with version check`` () =
        let sut = KeyValueStore<_, _> ()
        let unknownKey = Guid.NewGuid()
        let dummyVersion = "\"1\"" |> Version.parse
        
        match sut.Remove (unknownKey, dummyVersion) with
        | Error (RemoveError.KeyNotFound errorKey) -> Assert.AreEqual<_> (unknownKey, errorKey)
        | Error _ -> failNamedErrorExpected (nameof RemoveError.KeyNotFound)
        | Ok _ -> failErrorExpected ()

    [<TestMethod>]
    member _.``VersionMismatch Error Remove`` () =
        let sut = KeyValueStore<_, _> ()
        let key = Guid.NewGuid()
        let (_, _, initialVersion) = sut.Add (key, "A") |> okValue
        sut.Update (key, "B", Some initialVersion) |> ignore

        match sut.Remove (key, Some initialVersion) with
        | Error (RemoveError.VersionMismatch errorKey) -> Assert.AreEqual<_> (key, errorKey)
        | Error _ -> failNamedErrorExpected (nameof RemoveError.VersionMismatch)
        | Ok _ -> failErrorExpected ()
