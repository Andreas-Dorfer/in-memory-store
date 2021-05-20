namespace FTGO.Common.BaseTypes

open FTGO.Common
open FTGO.Common.Operators.OptionOperators

[<Struct>]
type NonEmptyString = private NonEmptyString of string

module NonEmptyString =

    let create = StringValidation.minLength 1 >|> NonEmptyString
