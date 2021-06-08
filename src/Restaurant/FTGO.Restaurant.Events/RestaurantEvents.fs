namespace FTGO.Restaurant.Events

open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes

type RestaurantCreatedEvent = {
    Id : RestaurantId
    Name : NonEmptyString
}
