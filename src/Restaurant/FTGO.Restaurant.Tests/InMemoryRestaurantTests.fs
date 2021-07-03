namespace FTGO.Restaurant.Tests

open Microsoft.VisualStudio.TestTools.UnitTesting
open FTGO.Restaurant.BaseTypes
open FTGO.Restaurant.Entities
open AD.InMemoryStore.Functional
open FTGO.Restaurant.InMemoryEntities

[<TestClass>]
type InMemoryRestaurantTests () =
    inherit RestaurantTests<InMemoryStore<RestaurantId, RestaurantEntity>> ()

    static do
        RestaurantTests<_>.Init ()

    override _.Context () = new TestDependency<_> (InMemoryStore<_, _> ())

    override _.CreateEntityDependency store =
        let createEntity = RestaurantAggregateAdapter.create store
        new TestDependency<_> (createEntity)

    override _.ReadEntityDependency store =
        let readEntity = RestaurantAggregateAdapter.read store
        new TestDependency<_> (readEntity)
