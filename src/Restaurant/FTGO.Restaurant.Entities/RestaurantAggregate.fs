namespace FTGO.Restaurant.Entities

open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes
open FTGO.Restaurant.Events

type CreateMenuItemEntityArgs = {
    Name : NonEmptyString
    Price : decimal
}

type MenuItemEntity = {
    Id : MenuItemId
    Name : NonEmptyString
    Price : decimal
}


type CreateRestaurantEntityArgs = {
    Name : NonEmptyString
    Menu : CreateMenuItemEntityArgs list
}

type RestaurantEntity = {
    Id :  EntityId<RestaurantId>
    Name : NonEmptyString
    Menu : MenuItemEntity list
}


type CreateRestaurantEntity = (CreateRestaurantEntityArgs * RestaurantCreatedEvent) -> Async<RestaurantEntity>

type FindRestaurantEntity = RestaurantId -> Async<RestaurantEntity option>
