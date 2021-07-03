namespace FTGO.Restaurant.InMemoryEntities

open System
open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes
open FTGO.Restaurant.Events
open FTGO.Restaurant.Entities
open AD.InMemoryStore
open AD.InMemoryStore.Functional

type Stores = {
    Restaurants : InMemoryStore<RestaurantId, RestaurantEntity>
    Events : InMemoryStore<Guid, RestaurantCreatedEvent>
}

module Stores =
    let create () = {
        Restaurants = InMemoryStore<RestaurantId, RestaurantEntity> ()
        Events = InMemoryStore<Guid, RestaurantCreatedEvent> ()
    }

module RestaurantAggregateAdapter =

    let private toEtag (version : Version) = version.ToString() |> ETag

    let create stores : CreateRestaurantEntity =
        fun (restaurant, created) -> async {
            match (restaurant.Id, restaurant) |> stores.Restaurants.Add with
            | Ok (restaurant, version) ->
                match (created.Id, created) |> stores.Events.Add with
                | Ok _ -> return Versioned (restaurant, version |> toEtag)
                | Error (AddError.DuplicateKey key) -> return invalidArg (nameof created) (key.ToString())
            | Error (AddError.DuplicateKey key) -> return invalidArg (nameof restaurant) (key.ToString())
        }

    let read { Restaurants = restaurants } : ReadRestaurantEntity =
        fun id -> async {
            match id |> restaurants.Get with
            | Ok (restaurant, version) -> return Some (Versioned (restaurant, version |> toEtag))
            | Error (GetError.KeyNotFound _) -> return None
        }
