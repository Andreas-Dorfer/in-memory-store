namespace FTGO.Common.BaseTypes

open FTGO.Common

[<Struct>]
type NonEmptyString = private NonEmptyString of string

module NonEmptyString =

    let create = StringValidation.minLength 1 >> Option.map NonEmptyString
