namespace FTGO.Restaurant

open System
open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes
open FTGO.Restaurant.Events
open FTGO.Restaurant.Entities
open FTGO.Restaurant.UseCases

module RestaurantService =

    let private newRestaurantId = Guid.NewGuid >> RestaurantId.create >> Option.get

    let private fromEntity (Versioned (entity : RestaurantEntity, eTag)) = {
        Id = Versioned (entity.Id, eTag)
        Name = entity.Name
    }

    let create (createEntity : CreateRestaurantEntity) : CreateRestaurant =
        fun args -> async {
            let entity = {
                Id = newRestaurantId ()
                Name = args.Name
                Menu = [] }
            let created : RestaurantCreatedEvent = {
                Id = Guid.NewGuid()
                RestaurantId = entity.Id
                Name = entity.Name
            }
            let! eTag = (entity, created) |> createEntity
            return Versioned (entity, eTag) |> fromEntity
        }

    let read (readEntity : ReadRestaurantEntity) : ReadRestaurant =
        fun id -> async {
            let! entity = id |> readEntity
            return entity |> Option.map fromEntity
        }
