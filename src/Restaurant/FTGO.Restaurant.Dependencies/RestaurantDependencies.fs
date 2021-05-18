namespace FTGO.Restaurant.Dependencies

open FTGO.Restaurant.BaseTypes
open FTGO.Restaurant.Entities
open FTGO.Restaurant.Events

type CreateRestaurantEntity = (CreateRestaurantEntityArgs * RestaurantCreatedEvent) -> Async<RestaurantEntity>

type FindRestaurantEntity = RestaurantId -> Async<RestaurantEntity option>
