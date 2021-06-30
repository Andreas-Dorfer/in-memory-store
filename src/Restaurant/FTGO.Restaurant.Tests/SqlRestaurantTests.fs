namespace FTGO.Restaurant.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Microsoft.EntityFrameworkCore
open FTGO.Restaurant.SqlEntities
open FTGO.Restaurant.SqlEntities.Core

[<TestClass>]
type SqlRestaurantTests () =
    inherit RestaurantTests<DbContextOptions> ()

    static do
        RestaurantTests<_>.Init ()

    override _.Context () =
        let options = DbContextOptionsBuilder<RestaurantDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options
        new TestDependency<_> (options)

    override _.CreateEntityDependency options =
        let context = new RestaurantDbContext (options)
        let createEntity = RestaurantAggregateAdapter.create context
        new TestDependency<_> (createEntity, context)

    override _.ReadEntityDependency options =
        let context = new RestaurantDbContext (options)
        let readEntity = RestaurantAggregateAdapter.read context
        new TestDependency<_> (readEntity, context)
