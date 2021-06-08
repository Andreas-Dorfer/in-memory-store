namespace FTGO.Restaurant.CosmosDbEntities

open FTGO.Common.BaseTypes
open FTGO.Restaurant.Entities
open FTGO.Restaurant.CosmosDbEntities.Core

module RestaurantAggregateAdapter =

    let create container : CreateRestaurantEntity =
        let dataAccess = container |> RestaurantDataAccess
        fun (restaurant, created) -> async {

            let entity = Restaurant ()
            entity.Id <- restaurant.Id.Value
            entity.Name <- restaurant.Name.Value

            let event = RestaurantCreated ()
            event.Id <- created.Id
            event.Name <- created.Name.Value

            let! entity = (entity, event) |> dataAccess.Create |> Async.AwaitTask

            return ETag entity.ETag
        }
