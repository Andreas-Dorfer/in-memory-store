namespace FTGO.Restaurant.UseCases

open FTGO.Restaurant.BaseTypes

type Restaurant = {
    Id : RestaurantId
    Name : string
}

type FindRestaurant = RestaurantId -> Restaurant option
