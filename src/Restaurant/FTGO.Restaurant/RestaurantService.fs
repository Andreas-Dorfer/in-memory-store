namespace FTGO.Restaurant

open System
open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes
open FTGO.Restaurant.Events
open FTGO.Restaurant.Entities
open FTGO.Restaurant.UseCases

module RestaurantService =

    let private newMenuItemId = Guid.NewGuid >> MenuItemId.create >> Option.get
    let private newRestaurantId = Guid.NewGuid >> RestaurantId.create >> Option.get

    let private createNewMenuItem (args : CreateMenuItemArgs) = {
        Id = newMenuItemId ()
        Name = args.Name
        Price = args.Price
    }

    let private createNewRestaurant (args : CreateRestaurantArgs) =
        let restaurant = {
            Id = newRestaurantId ()
            Name = args.Name
            Menu = args.Menu |> List.map createNewMenuItem }
        let created : RestaurantCreatedEvent = {
            Id = Guid.NewGuid()
            RestaurantId = restaurant.Id
            Name = restaurant.Name
        }
        restaurant, created

    let private fromEntity (Versioned (entity : RestaurantEntity, eTag)) = {
        Id = Versioned (entity.Id, eTag)
        Name = entity.Name
    }


    let create (createEntity : CreateRestaurantEntity) : CreateRestaurant =
        fun args -> async {
            let (entity, created) = args |> createNewRestaurant
            let! eTag = (entity, created) |> createEntity
            return Versioned (entity, eTag) |> fromEntity
        }

    let read (readEntity : ReadRestaurantEntity) : ReadRestaurant =
        fun id -> async {
            let! entity = id |> readEntity
            return entity |> Option.map fromEntity
        }
