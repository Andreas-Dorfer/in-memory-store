namespace FTGO.Restaurant.Conversions

module Restaurant =

    type private Entity = FTGO.Restaurant.Entities.RestaurantEntity
    type private UseCase = FTGO.Restaurant.UseCases.Restaurant

    let fromEntity (entity : Entity) : UseCase = {
        Id = entity.Id
        Name = entity.Name
    }
