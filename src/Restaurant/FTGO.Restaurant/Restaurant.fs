namespace FTGO.Restaurant

open FTGO.Restaurant.Entities
open FTGO.Restaurant.UseCases

module Restaurant =

    let find (findEntity : FindRestaurantEntity) : FindRestaurant =
        let toRestaurant (entity : RestaurantEntity) = {
            Id = entity.Id
            Name = entity.Name
        }
        findEntity >> Option.map toRestaurant
