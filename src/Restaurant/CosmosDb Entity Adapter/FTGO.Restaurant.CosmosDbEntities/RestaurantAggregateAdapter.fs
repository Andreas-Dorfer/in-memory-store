namespace FTGO.Restaurant.CosmosDbEntities

open System
open System.Threading.Tasks
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
            restaurant.Menu
            |> Seq.map (fun m ->
                let menuItem = MenuItem ()
                menuItem.Name <- m.Name.Value
                menuItem.Price <- m.Price
                menuItem)
            |> entity.Menu.AddRange

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
                        Menu = entity.Menu |> Seq.map (fun m -> { Id = Guid.Parse m.Id |> MenuItemId.create |> Option.get; Name = m.Name |> NonEmptyString.create |> Option.get; Price = m.Price }) |> Seq.toList
                    }
                    Versioned (restaurant, ETag entity.ETag))
        }

    let startMessageFeedProcessor container leaseContainer f =
        let dataAccess = container |> RestaurantDataAccess
        let onNewMessage message = async {
            do! message |> f
        }

        let processor = dataAccess.CreateMessageFeedProcessor(leaseContainer, fun message -> message |> onNewMessage |> Async.StartAsTask :> Task)
        async {
            do! processor.StartAsync () |> Async.AwaitTask
            return {| Stop = processor.StopAsync >> Async.AwaitTask |}
        }
