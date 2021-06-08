namespace FTGO.Restaurant.BaseTypes

open System
open FTGO.Common

[<Struct>]
type MenuItemId = private MenuItemId of Guid

module MenuItemId =

    let create = GuidValidation.notEmpty >> Option.map MenuItemId
