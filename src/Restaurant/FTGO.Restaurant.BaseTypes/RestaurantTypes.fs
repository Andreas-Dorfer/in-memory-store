namespace FTGO.Restaurant.BaseTypes

open System
open FTGO.Common

[<Struct>]
type RestaurantId = private RestaurantId of Guid
    with member id.Value = let (RestaurantId value) = id in value

module RestaurantId =

    let create = GuidValidation.notEmpty >> Option.map RestaurantId
