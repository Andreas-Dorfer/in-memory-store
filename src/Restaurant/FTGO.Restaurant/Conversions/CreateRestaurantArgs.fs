namespace FTGO.Restaurant.Conversions

module CreateRestaurantArgs =

    type private UseCaseArgs = FTGO.Restaurant.UseCases.CreateRestaurantArgs
    type private EntityArgs = FTGO.Restaurant.Entities.CreateRestaurantEntityArgs

    let toEntityArgs (args : UseCaseArgs) : EntityArgs = {
        Name = args.Name
        Menu = []
    }
