namespace FTGO.Restaurant.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FsCheck
open FTGO.Restaurant.Entities
open FTGO.Restaurant
open RestaurantBehavior

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
