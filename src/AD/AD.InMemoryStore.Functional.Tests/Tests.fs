namespace AD.InMemoryStore.Functional.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open AD.InMemoryStore.Functional

[<TestClass>]
type TestClass () =

    let failOkExpected () = Assert.Fail "Ok expected"
    let failErrorExpected () = Assert.Fail "Error expected"

    let okValue = function
        | Ok ok -> ok
        | Error _ -> raise (AssertFailedException "Ok expected")

    [<TestMethod>]
    member _.``Ok Add`` () =
        let sut = InMemoryStore<_, _> ()
        let expectedKey = Guid.NewGuid()
        let expectedValue = "A"

        match sut.Add (expectedKey, expectedValue) with
        | Ok (actualKey, actualValue, _) ->
            Assert.AreEqual<_> (expectedKey, actualKey)
            Assert.AreEqual<_> (expectedValue, actualValue)
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
        let expectedKey = Guid.NewGuid()
        let expected = sut.Add (expectedKey, "A")

        match sut.Get(expectedKey) with
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
        fun (values : Map<Guid, string>) ->
            let sut = InMemoryStore<_, _> ()
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
        let sut = InMemoryStore<_, _> ()
        let expectedKey = Guid.NewGuid()
        let (_, _, initialVersion) = sut.Add (expectedKey, "A") |> okValue
        let expectedValue = "B"

        match sut.Update (expectedKey, expectedValue, Some initialVersion) with
        | Ok (actualKey, actualValue, actualVersion) ->
            Assert.AreEqual<_> (expectedKey, actualKey)
            Assert.AreEqual<_> (expectedValue, actualValue)
            Assert.AreNotEqual<_> (initialVersion, actualVersion)
        | Error _ -> failOkExpected ()
