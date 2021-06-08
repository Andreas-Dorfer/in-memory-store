namespace FTGO.Restaurant.BaseTypes

open System
open FTGO.Common

[<Struct>]
type RestaurantId = private RestaurantId of Guid

module RestaurantId =

    let create = GuidValidation.notEmpty >> Option.map RestaurantId
