namespace FTGO.Restaurant.Tests

open System.Collections.Generic
open System.Threading.Tasks
open Microsoft.VisualStudio.TestTools.UnitTesting
open FTGO.Common.BaseTypes
open FTGO.Restaurant.Events
open FTGO.Restaurant.UseCases
open FTGO.Restaurant

[<TestClass>]
type CreateTests () =

    [<TestMethod>]
    member _.Foo () =
        async {
            let restaurants = List<_> ()
            let events = Queue<RestaurantCreatedEvent> ()
            let createEntity (restaurant, entityCreated) = async {
                restaurant |> restaurants.Add
                entityCreated |> events.Enqueue
                return Versioned (restaurant, ETag "1")
            }
            let createRestaurant = RestaurantService.create createEntity
            
            let expectedName = NonEmptyString.create "My Restaurant" |> Option.get
            let args : CreateRestaurantArgs = { Name = expectedName }

            let! restaurant = args |> createRestaurant
            
            Assert.AreEqual<_> (expectedName, restaurant.Name)
            Assert.AreEqual<_> (1, events.Count)
        }
        |> Async.StartAsTask :> Task
