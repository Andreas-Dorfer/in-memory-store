namespace FTGO.Restaurant.Tests

open FTGO.Restaurant.UseCases

module RestaurantTests =

    let private toArgs (restaurant : Restaurant) = {
        Name = restaurant.Name
        Menu = restaurant.Menu |> List.map (fun menuItem -> {
            Name = menuItem.Name
            Price = menuItem.Price })
    }

    let ``create a restaurant`` (create : CreateRestaurant) args = async {
        let! restaurant = args |> create
        return args = (restaurant |> toArgs)
    }

    let ``read a restaurant`` (create : CreateRestaurant) (read : ReadRestaurant) args = async {
        let! expected = args |> create
        let! actual = expected.Id.Value |> read
        return Some expected = actual
    }

    let ``read an unknown restaurant`` (read : ReadRestaurant) unknownId = async {
        let! result = unknownId |> read
        return None = result
    }


open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open FTGO.Restaurant.Entities
open FTGO.Restaurant
open RestaurantTests

[<AbstractClass>]
type RestaurantTests () =

    abstract member CreateEntityDependency : unit -> TestDependency<CreateRestaurantEntity>
    abstract member ReadEntityDependency : unit -> TestDependency<ReadRestaurantEntity>

    static member Init () =
        Arb.register<Generators> () |> ignore

    [<TestMethod>]
    member test.``create a restaurant`` () =
        fun args -> async {
            use createEntityDependency = test.CreateEntityDependency ()
            let create = RestaurantService.create createEntityDependency.Value
            return! args |> ``create a restaurant`` create
        }
        >> Async.RunSynchronously
        |> Check.QuickThrowOnFailure

    [<TestMethod>]
    member test.``read a restaurant`` () =
        fun args -> async {
            use createEntityDependency = test.CreateEntityDependency ()
            use readEntityDependency = test.ReadEntityDependency ()
            let create = RestaurantService.create createEntityDependency.Value
            let read = RestaurantService.read readEntityDependency.Value
            return! args |> ``read a restaurant`` create read
        }
        >> Async.RunSynchronously
        |> Check.QuickThrowOnFailure

    [<TestMethod>]
    member test.``read an unknown restaurant`` () =
        fun unknownId -> async {
            use readEntityDependency = test.ReadEntityDependency ()
            let read = RestaurantService.read readEntityDependency.Value
            return! unknownId |> ``read an unknown restaurant`` read
        }
        >> Async.RunSynchronously
        |> Check.QuickThrowOnFailure


open Microsoft.Azure.Cosmos
open FTGO.Restaurant.CosmosDbEntities

[<TestClass>]
type CosmosDbRestaurantTests () =
    inherit RestaurantTests ()

    static do
        RestaurantTests.Init ()

    [<Literal>]
    let endpoint = "https://localhost:8081"
    [<Literal>]
    let primaryKey = "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw=="

    let getContainer () =
        let client = new CosmosClient(endpoint, primaryKey)
        let db = client.GetDatabase("RestaurantService")
        db.GetContainer("Restaurant"), client

    override _.CreateEntityDependency () =
        let (container, client) = getContainer ()
        let createEntity = RestaurantAggregateAdapter.create container
        TestDependency (createEntity, client)

    override _.ReadEntityDependency () =
        let (container, client) = getContainer ()
        let createEntity = RestaurantAggregateAdapter.read container
        TestDependency (createEntity, client)
