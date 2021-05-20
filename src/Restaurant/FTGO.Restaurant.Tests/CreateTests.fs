namespace FTGO.Restaurant.Tests

open System
open System.Collections.Generic
open System.Threading.Tasks
open Microsoft.VisualStudio.TestTools.UnitTesting
open FTGO.Common.BaseTypes
open FTGO.Restaurant.BaseTypes
open FTGO.Restaurant.Entities
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
            let createEntity entityCreated (args : CreateRestaurantEntityArgs) = async {
                let restaurant = {
                    Id = EntityId (Guid.NewGuid().ToString() |> RestaurantId.create |> Option.get, "1")
                    Name = args.Name
                    Menu = []
                }
                restaurant |> restaurants.Add
                restaurant |> entityCreated |> events.Enqueue
                return restaurant
            }
            let createRestaurant = RestaurantService.create createEntity
            
            let expectedName = NonEmptyString.create "My Restaurant" |> Option.get
            let args : CreateRestaurantArgs = { Name = expectedName }

            let! restaurant = args |> createRestaurant
            
            Assert.AreEqual<_> (expectedName, restaurant.Name)
            Assert.AreEqual<_> (1, events.Count)
        }
        |> Async.StartAsTask :> Task
