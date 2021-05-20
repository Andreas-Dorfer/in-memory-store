namespace FTGO.Restaurant.BaseTypes

open FTGO.Common
open FTGO.Common.Operators.OptionOperators

[<Struct>]
type MenuItemId = private MenuItemId of string

module MenuItemId =

    let create = StringValidation.minLength 1 >|> MenuItemId
