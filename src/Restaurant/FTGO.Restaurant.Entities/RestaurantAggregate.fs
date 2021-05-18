namespace FTGO.Restaurant.Entities

open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes

type MenuItemEntity = {
    Id : MenuItemId
    Name : NonEmptyString
    Price : decimal
}

type RestaurantEntity = {
    Id :  RestaurantId
    Name : NonEmptyString
    Menu : MenuItemEntity list
}

type FindRestaurantEntity = RestaurantId -> Async<RestaurantEntity option>
