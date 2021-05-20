namespace FTGO.Restaurant.Entities

open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes
open FTGO.Common.Entities
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

type RestaurantEntity =
    {
        Id :  EntityId<RestaurantId>
        Name : NonEmptyString
        Menu : MenuItemEntity list
    }
    interface IEntity<RestaurantId> with
        member this.Id = this.Id


type RestaurantEntityCreated = RestaurantEntity -> RestaurantCreatedEvent

type CreateRestaurantEntity = RestaurantEntityCreated -> CreateRestaurantEntityArgs -> Async<RestaurantEntity>

type FindRestaurantEntity = RestaurantId -> Async<RestaurantEntity option>
