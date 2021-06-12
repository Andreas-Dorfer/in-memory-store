namespace FTGO.Restaurant.Events

open System
open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes

type MenuItem = {
    Id : MenuItemId
    Name : NonEmptyString
    Price : decimal
}

type RestaurantCreatedEvent = {
    Id : Guid
    RestaurantId : RestaurantId
    Name : NonEmptyString
    Menu : MenuItem list
}
