namespace FTGO.Restaurant.UseCases

open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes

type CreateMenuItemArgs = {
    Name : NonEmptyString
    Price : decimal
}

type CreateRestaurantArgs = {
    Name : NonEmptyString
    Menu : CreateMenuItemArgs list
}

type MenuItem = {
    Id : Versioned<MenuItemId>
    Name : NonEmptyString
    Price : decimal
}

type Restaurant = {
    Id : Versioned<RestaurantId>
    Name : NonEmptyString
    Menu : MenuItem list
}

type CreateRestaurant = CreateRestaurantArgs -> Async<Restaurant>

type ReadRestaurant = RestaurantId -> Async<Restaurant option>
