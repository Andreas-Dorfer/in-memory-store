namespace FTGO.Restaurant.Entities

open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes

type MenuItem = {
    Id : MenuItemId
    Name : NonEmptyString
    Price : decimal
}

type RestaurantEntity = {
    Id :  RestaurantId
    Name : NonEmptyString
    Menu : MenuItem list
}
