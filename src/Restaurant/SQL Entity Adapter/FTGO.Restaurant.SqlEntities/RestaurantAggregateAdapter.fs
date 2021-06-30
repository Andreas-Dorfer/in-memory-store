namespace FTGO.Restaurant.SqlEntities

open System
open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes
open FTGO.Restaurant.Entities
open FTGO.Restaurant.SqlEntities.Core

module RestaurantAggregateAdapter =

    let private ofSqlEntity (entity : Restaurant) = {
        Id = entity.Id |> RestaurantId.create |> Option.get
        Name = entity.Name |> NonEmptyString.create |> Option.get
        Menu = entity.Menu |> Seq.map (fun m -> { Id = m.Id |> MenuItemId.create |> Option.get; Name = m.Name |> NonEmptyString.create |> Option.get; Price = m.Price }) |> Seq.toList
    }

    let private toEtag timestamp =
        "\"1\"" |> ETag
        //timestamp |> Convert.ToBase64String |> sprintf "\"%s\"" |> ETag

    let create context : CreateRestaurantEntity =
        let dataAccess = context |> RestaurantDataAccess
        fun (restaurant, created) -> async {

            let entity = Restaurant ()
            entity.Id <- restaurant.Id.Value
            entity.Name <- restaurant.Name.Value
            restaurant.Menu
            |> Seq.map (fun menuItem ->
                let menuItemEntity = MenuItem ()
                menuItemEntity.Id <- menuItem.Id.Value
                menuItemEntity.Name <- menuItem.Name.Value
                menuItemEntity.Price <- menuItem.Price
                menuItemEntity)
            |> entity.Menu.AddRange

            let event = RestaurantCreated ()
            event.Id <- created.Id
            event.Name <- created.Name.Value

            let! entity = (entity, event) |> dataAccess.Create |> Async.AwaitTask

            return Versioned (entity |> ofSqlEntity, entity.RowVersion |> toEtag)
        }

    let read context : ReadRestaurantEntity =
        let dataAccess = context |> RestaurantDataAccess
        fun id -> async {
            let! entity = id.Value |> dataAccess.Read |> Async.AwaitTask

            return
                entity
                |> Option.ofObj
                |> Option.map (fun entity ->
                    let restaurant = {
                        Id = entity.Id |> RestaurantId.create |> Option.get
                        Name = entity.Name |> NonEmptyString.create |> Option.get
                        Menu = entity.Menu |> Seq.map (fun m -> { Id = m.Id |> MenuItemId.create |> Option.get; Name = m.Name |> NonEmptyString.create |> Option.get; Price = m.Price }) |> Seq.toList
                    }
                    Versioned (restaurant, entity.RowVersion |> toEtag))
        }
