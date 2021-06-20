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
type RestaurantTests<'context> () =

    let check property = property >> Async.RunSynchronously |> Check.QuickThrowOnFailure

    abstract member Context : unit -> TestDependency<'context>
    abstract member CreateEntityDependency : 'context -> TestDependency<CreateRestaurantEntity>
    abstract member ReadEntityDependency : 'context -> TestDependency<ReadRestaurantEntity>

    static member Init () =
        Arb.register<Generators> () |> ignore

    [<TestMethod>]
    member test.``create a restaurant`` () =
        use context = test.Context ()
        use createEntityDependency = context.Value |> test.CreateEntityDependency
        let create = RestaurantService.create createEntityDependency.Value
        
        create |> ``create a restaurant`` |> check

    [<TestMethod>]
    member test.``read a restaurant`` () =
        use context = test.Context ()
        use createEntityDependency = context.Value |> test.CreateEntityDependency
        use readEntityDependency = context.Value |> test.ReadEntityDependency
        let create = RestaurantService.create createEntityDependency.Value
        let read = RestaurantService.read readEntityDependency.Value
        
        (create, read) ||> ``read a restaurant`` |> check

    [<TestMethod>]
    member test.``read an unknown restaurant`` () =
        use context = test.Context ()
        use readEntityDependency = context.Value |> test.ReadEntityDependency
        let read = RestaurantService.read readEntityDependency.Value
        
        read |> ``read an unknown restaurant`` |> check


open Microsoft.Azure.Cosmos
open FTGO.Restaurant.CosmosDbEntities

[<TestClass>]
type CosmosDbRestaurantTests () =
    inherit RestaurantTests<Container> ()

    static do
        RestaurantTests<_>.Init ()

    override _.Context () = CosmosDbTest.CreateEntityContainer ()

    override _.CreateEntityDependency container =
        let createEntity = RestaurantAggregateAdapter.create container
        new TestDependency<_> (createEntity)

    override _.ReadEntityDependency container =
        let readEntity = RestaurantAggregateAdapter.read container
        new TestDependency<_> (readEntity)
