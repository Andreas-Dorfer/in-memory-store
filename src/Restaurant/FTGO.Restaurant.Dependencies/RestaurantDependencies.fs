namespace FTGO.Restaurant.Dependencies

open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes
open FTGO.Restaurant.Entities
open FTGO.Restaurant.Events

type CreateRestaurantEntityArgs = {
    Name : NonEmptyString
}

type CreateRestaurantEntity = (CreateRestaurantEntityArgs * RestaurantCreatedEvent) -> Async<RestaurantEntity>

type FindRestaurantEntity = RestaurantId -> Async<RestaurantEntity option>
