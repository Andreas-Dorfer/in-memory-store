namespace FTGO.Restaurant.Tests

open FsCheck
open FTGO.Common.BaseTypes

type Generators =

    static member NonEmptyString () =
        let map (FsCheck.NonEmptyString value) = value |> NonEmptyString.create |> Option.get
        { new Arbitrary<_> () with
            override _.Generator = Arb.generate |> Gen.map map
            override _.Shrinker value = value.Value |> FsCheck.NonEmptyString |> Arb.shrink |> Seq.map map }
