namespace FTGO.Restaurant

open FTGO.Restaurant.Entities
open FTGO.Restaurant.UseCases

module Restaurant =

    let fromEntity (entity : RestaurantEntity) : Restaurant = {
        Id = entity.Id
        Name = entity.Name
    }
