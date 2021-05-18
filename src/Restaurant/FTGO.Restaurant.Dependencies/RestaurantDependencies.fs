namespace FTGO.Restaurant.Dependencies

open FTGO.Restaurant.BaseTypes
open FTGO.Restaurant.Entities

type CreateRestaurantEntity = CreateRestaurantEntityArgs -> Async<RestaurantEntity>

type FindRestaurantEntity = RestaurantId -> Async<RestaurantEntity option>
