namespace FTGO.Restaurant.Tests

open Swensen.Unquote
open FTGO.Restaurant.UseCases

module RestaurantBehavior =

    let private toArgs (restaurant : Restaurant) = {
        Name = restaurant.Name
        Menu = restaurant.Menu |> List.map (fun menuItem -> {
            Name = menuItem.Name
            Price = menuItem.Price })
    }

    let ``create a restaurant`` (create : CreateRestaurant) args = async {
        let! restaurant = args |> create
        return args =! (restaurant |> toArgs)
    }

    let ``read a restaurant`` (create : CreateRestaurant) (read : ReadRestaurant) args = async {
        let! expected = args |> create
        let! actual = expected.Id.Value |> read
        return Some expected =! actual
    }

    let ``read an unknown restaurant`` (read : ReadRestaurant) unknownId = async {
        let! result = unknownId |> read
        return None =! result
    }
