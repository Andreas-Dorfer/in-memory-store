namespace FTGO.Restaurant.Entities

open FTGO.Restaurant.BaseTypes

type MenuItem = {
    Id : MenuItemId
    Name : string
    Price : decimal
}

type RestaurantEntity = {
    Id :  RestaurantId
    Name : string
    Menu : MenuItem list
}
