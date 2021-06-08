namespace FTGO.Restaurant.Entities

open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes
open FTGO.Restaurant.Events

type MenuItemEntity = {
    Id : MenuItemId
    Name : NonEmptyString
    Price : decimal
}

type RestaurantEntity = {
    Id : RestaurantId
    Name : NonEmptyString
    Menu : MenuItemEntity list
}

type CreateRestaurantEntity = RestaurantEntity * RestaurantCreatedEvent -> Async<ETag>

type ReadRestaurantEntity = RestaurantId -> Async<Versioned<RestaurantEntity> option>
