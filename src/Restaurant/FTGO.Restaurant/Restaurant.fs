namespace FTGO.Restaurant

open FTGO.Restaurant.Entities
open FTGO.Restaurant.Events
open FTGO.Restaurant.UseCases
open FTGO.Common.Operators.OptionOperators
open FTGO.Restaurant.Conversions

module Restaurant =

    let create (createEntity : CreateRestaurantEntity) : CreateRestaurant =
        let toCreateEntityArgs createArgs =
            let createEntityArgs = createArgs |> CreateRestaurantArgs.toEntityArgs

            let created : RestaurantCreatedEvent = {
                Name = createArgs.Name
            }
            createEntityArgs, created

        fun args -> async {
            let! entity = args |> toCreateEntityArgs |> createEntity
            return entity |> Restaurant.fromEntity
        }

    let find (findEntity : FindRestaurantEntity) : FindRestaurant =
        fun id -> async {
            let! entity = id |> findEntity
            return entity |>> Restaurant.fromEntity
        }
