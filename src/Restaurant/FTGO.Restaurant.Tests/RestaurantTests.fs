namespace FTGO.Restaurant.Tests

open Swensen.Unquote
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


open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open FTGO.Restaurant.Entities
open FTGO.Restaurant
open RestaurantTests

[<AbstractClass>]
type RestaurantTests () =

    let check property = property >> Async.RunSynchronously |> Check.QuickThrowOnFailure

    abstract member CreateEntityDependency : unit -> TestDependency<CreateRestaurantEntity>
    abstract member ReadEntityDependency : unit -> TestDependency<ReadRestaurantEntity>

    static member Init () =
        Arb.register<Generators> () |> ignore

    [<TestMethod>]
    member test.``create a restaurant`` () =
        use createEntityDependency = test.CreateEntityDependency ()
        let create = RestaurantService.create createEntityDependency.Value
        
        create |> ``create a restaurant`` |> check

    [<TestMethod>]
    member test.``read a restaurant`` () =
        use createEntityDependency = test.CreateEntityDependency ()
        use readEntityDependency = test.ReadEntityDependency ()
        let create = RestaurantService.create createEntityDependency.Value
        let read = RestaurantService.read readEntityDependency.Value
        
        (create, read) ||> ``read a restaurant`` |> check

    [<TestMethod>]
    member test.``read an unknown restaurant`` () =
        use readEntityDependency = test.ReadEntityDependency ()
        let read = RestaurantService.read readEntityDependency.Value
        
        read |> ``read an unknown restaurant`` |> check


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
