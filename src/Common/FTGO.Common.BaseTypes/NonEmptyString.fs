namespace FTGO.Common.BaseTypes

open FTGO.Common

[<Struct>]
type NonEmptyString = private NonEmptyString of string
    with member this.Value = let (NonEmptyString value) = this in value

module NonEmptyString =

    let create = StringValidation.minLength 1 >> Option.map NonEmptyString
