namespace FTGO.Restaurant.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FTGO.Restaurant.InMemoryEntities

[<TestClass>]
type InMemoryRestaurantTests () =
    inherit RestaurantTests<Stores> ()

    static do
        RestaurantTests<_>.Init ()

    override _.Context () = new TestDependency<_> (Stores.create ())

    override _.CreateEntityDependency stores =
        let createEntity = RestaurantAggregateAdapter.create stores
        new TestDependency<_> (createEntity)

    override _.ReadEntityDependency stores =
        let readEntity = RestaurantAggregateAdapter.read stores
        new TestDependency<_> (readEntity)
