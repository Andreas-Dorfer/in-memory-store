namespace FTGO.Restaurant

open System
open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes
open FTGO.Restaurant.Entities
open FTGO.Restaurant.Events
open FTGO.Restaurant.UseCases

module RestaurantService =

    let create (createEntity : CreateRestaurantEntity) : CreateRestaurant =
        fun args -> async {
            let entity = {
                Id = Guid.NewGuid().ToString() |> RestaurantId.create |> Option.get
                Name = args.Name
                Menu = [] }
            let event : RestaurantCreatedEvent = {
                Id = entity.Id
                Name = entity.Name
            }
            let! Versioned (entity, eTag) = (entity, event) |> createEntity
            return {
                Id = Versioned (entity.Id, eTag)
                Name = entity.Name
            }
        }

    let read (readEntity : ReadRestaurantEntity) : ReadRestaurant =
        fun id -> async {
            let! result = id |> readEntity
            return
                result
                |> Option.map (fun (Versioned (entity, eTag)) -> {
                    Id = Versioned (entity.Id, eTag)
                    Name = entity.Name
                })
        }
