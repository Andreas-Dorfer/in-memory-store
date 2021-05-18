﻿namespace FTGO.Restaurant

open FTGO.Restaurant.Dependencies
open FTGO.Restaurant.Entities
open FTGO.Restaurant.Events
open FTGO.Restaurant.UseCases

module Restaurant =

    let private toRestaurant (entity : RestaurantEntity) = {
        Id = entity.Id
        Name = entity.Name
    }

    let create (createEntity : CreateRestaurantEntity) : CreateRestaurant =
        let toCreateEntityArgs args =
            let createEntityArgs : CreateRestaurantEntityArgs = {
                Name = args.Name
            }
            let createdEvent : RestaurantCreatedEvent = {
                Name = args.Name
            }
            (createEntityArgs, createdEvent)

        fun args -> async {
            let! entity = args |> toCreateEntityArgs |> createEntity
            return entity |> toRestaurant
        }

    let find (findEntity : FindRestaurantEntity) : FindRestaurant =
        fun id -> async {
            let! entity = id |> findEntity
            return entity |> Option.map toRestaurant
        }
