namespace FTGO.Restaurant.CosmosDbEntities

open System
open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes
open FTGO.Restaurant.Entities
open FTGO.Restaurant.CosmosDbEntities.Core

module RestaurantAggregateAdapter =

    let create container : CreateRestaurantEntity =
        let dataAccess = container |> RestaurantDataAccess
        fun (restaurant, created) -> async {

            let entity = Restaurant ()
            entity.Id <- restaurant.Id.Value.ToString()
            entity.Name <- restaurant.Name.Value

            let event = RestaurantCreated ()
            event.Id <- created.Id.ToString()
            event.Name <- created.Name.Value

            let! entity = (entity, event) |> dataAccess.Create |> Async.AwaitTask

            return ETag entity.ETag
        }

    let read container : ReadRestaurantEntity =
        let dataAccess = container |> RestaurantDataAccess
        fun id -> async {
            let! entity = id.Value |> dataAccess.Read |> Async.AwaitTask

            return
                entity
                |> Option.ofObj
                |> Option.map (fun entity ->
                    let restaurant = {
                        Id = entity.Id |> Guid.Parse |> RestaurantId.create |> Option.get
                        Name = entity.Name |> NonEmptyString.create |> Option.get
                        Menu = []
                    }
                    Versioned (restaurant, ETag entity.ETag))
        }
