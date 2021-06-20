namespace FTGO.Restaurant.BaseTypes

open System
open FTGO.Common

[<Struct>]
type MenuItemId = private MenuItemId of Guid
    with member id.Value = let (MenuItemId value) = id in value

module MenuItemId =

    let create = GuidValidation.notEmpty >> Option.map MenuItemId
