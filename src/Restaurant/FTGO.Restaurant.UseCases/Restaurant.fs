namespace FTGO.Restaurant.UseCases

open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes

type Restaurant = {
    Id : RestaurantId
    Name : NonEmptyString
}

type FindRestaurant = RestaurantId -> Restaurant option
