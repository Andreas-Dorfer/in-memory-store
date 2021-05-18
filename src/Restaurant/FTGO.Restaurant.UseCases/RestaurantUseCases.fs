namespace FTGO.Restaurant.UseCases

open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes

type Restaurant = {
    Id : RestaurantId
    Name : NonEmptyString
}

type CreateRestaurantArgs = {
    Name : NonEmptyString
}

type CreateRestaurant = CreateRestaurantArgs -> Async<Restaurant>

type FindRestaurant = RestaurantId -> Async<Restaurant option>
