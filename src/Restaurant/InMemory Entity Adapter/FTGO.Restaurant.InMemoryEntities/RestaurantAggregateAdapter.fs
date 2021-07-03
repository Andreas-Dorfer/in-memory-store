namespace FTGO.Restaurant.InMemoryEntities

open FTGO.Common.BaseTypes
open FTGO.Restaurant.Entities
open AD.InMemoryStore
open AD.InMemoryStore.Functional

module RestaurantAggregateAdapter =

    let private toEtag (version : Version) = version.ToString() |> ETag

    let create (store : InMemoryStore<_, _>) : CreateRestaurantEntity =
        fun (restaurant, _) -> async {
            match (restaurant.Id, restaurant) |> store.Add with
            | Ok (restaurant, version) -> return Versioned (restaurant, version |> toEtag)
            | Error (AddError.DuplicateKey key) -> return invalidArg (nameof restaurant) (key.ToString())
        }

    let read (store : InMemoryStore<_, _>) : ReadRestaurantEntity =
        fun id -> async {
            match id |> store.Get with
            | Ok (restaurant, version) -> return Some (Versioned (restaurant, version |> toEtag))
            | Error (GetError.KeyNotFound _) -> return None
        }
