namespace FTGO.Restaurant

open FTGO.Restaurant.Entities
open FTGO.Restaurant.UseCases

module Restaurant =

    let private toRestaurant (entity : RestaurantEntity) = {
        Id = entity.Id
        Name = entity.Name
    }

    let create (createEntity : CreateRestaurantEntity) : CreateRestaurant =
        let toCreateEntityArgs args : CreateRestaurantEntityArgs = {
            Name = args.Name
        }

        fun args -> async {
            let! entity = args |> toCreateEntityArgs |> createEntity
            return entity |> toRestaurant
        }

    let find (findEntity : FindRestaurantEntity) : FindRestaurant =
        fun id -> async {
            let! entity = id |> findEntity
            return entity |> Option.map toRestaurant
        }
