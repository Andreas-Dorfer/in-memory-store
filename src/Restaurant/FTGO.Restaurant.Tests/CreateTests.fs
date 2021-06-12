namespace FTGO.Restaurant.Tests

open System.Threading.Tasks
open Microsoft.VisualStudio.TestTools.UnitTesting
open FTGO.Common.BaseTypes
open FTGO.Restaurant.UseCases
open FTGO.Restaurant
open FTGO.Restaurant.CosmosDbEntities
open Microsoft.Azure.Cosmos

[<TestClass>]
type CreateTests () =

    [<Literal>]
    let primaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="

    [<TestMethod>]
    member _.Foo () =
        async {
            use client = new CosmosClient("https://localhost:8081", primaryKey)
            let db = client.GetDatabase("RestaurantService")
            let container = db.GetContainer("Restaurant")

            let createEntity = RestaurantAggregateAdapter.create container
            let readEntity = RestaurantAggregateAdapter.read container

            let create = RestaurantService.create createEntity
            let read = RestaurantService.read readEntity

            
            let args = {
                Name = "My Restaurant" |> NonEmptyString.create |> Option.get
                Menu = []
            }
            let! createdRestaurant = args |> create
            let! readRestaurant = createdRestaurant.Id.Value |> read

            return ()
        }
        |> Async.StartAsTask :> Task
