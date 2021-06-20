namespace FTGO.Restaurant.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
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
