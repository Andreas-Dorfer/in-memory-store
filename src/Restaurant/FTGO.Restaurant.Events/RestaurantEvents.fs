namespace FTGO.Restaurant.Events

open System
open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes

type RestaurantCreatedEvent = {
    Id : Guid
    RestaurantId : RestaurantId
    Name : NonEmptyString
}
