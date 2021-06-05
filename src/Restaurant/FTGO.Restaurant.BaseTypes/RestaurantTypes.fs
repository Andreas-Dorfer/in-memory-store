namespace FTGO.Restaurant.BaseTypes

open FTGO.Common

[<Struct>]
type RestaurantId = private RestaurantId of string

module RestaurantId =

    let create = StringValidation.minLength 1 >> Option.map RestaurantId
