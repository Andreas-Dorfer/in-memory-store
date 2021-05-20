namespace FTGO.Restaurant

open FTGO.Restaurant.UseCases
open FTGO.Restaurant.Entities

module CreateRestaurantArgs =

    let toEntityArgs (args : CreateRestaurantArgs) : CreateRestaurantEntityArgs = {
        Name = args.Name
        Menu = []
    }
