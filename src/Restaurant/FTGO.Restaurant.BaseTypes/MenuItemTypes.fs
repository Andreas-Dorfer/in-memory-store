namespace FTGO.Restaurant.BaseTypes

open FTGO.Common

[<Struct>]
type MenuItemId = private MenuItemId of string

module MenuItemId =

    let create = StringValidation.minLength 1 >> Option.map MenuItemId
