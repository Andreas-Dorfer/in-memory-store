namespace FTGO.Restaurant

module CreateRestaurantArgs =

    let toCreateRestaurantEntityArgs (args : UseCases.CreateRestaurantArgs) : Entities.CreateRestaurantEntityArgs = {
        Name = args.Name
        Menu = []
    }
