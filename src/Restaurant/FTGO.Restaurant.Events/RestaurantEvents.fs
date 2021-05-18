namespace FTGO.Restaurant.Events

open FTGO.Common.BaseTypes

type RestaurantCreatedEvent = {
    Name : NonEmptyString
}
