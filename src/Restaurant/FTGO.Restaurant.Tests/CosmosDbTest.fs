namespace FTGO.Restaurant.Tests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open Microsoft.Azure.Cosmos
open AD.Messaging.Cosmos

[<TestClass>]
type CosmosDbTest =

    [<DefaultValue>]
    static val mutable private client : CosmosClient
    [<DefaultValue>]
    static val mutable private db : Database

    [<AssemblyInitialize>]
    static member Initialize (_ : TestContext) =
        CosmosDbTest.client <- new CosmosClient("https://localhost:8081", "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==")
        CosmosDbTest.db <- CosmosDbTest.client.CreateDatabaseAsync(Guid.NewGuid().ToString()).Result.Database

    static member Client = CosmosDbTest.client
    static member Database = CosmosDbTest.db

    static member CreateEntityContainer () =
        let container = CosmosDbTest.Database.CreateContainerAsync(Guid.NewGuid().ToString(), "/" + Entity.PartitionKey).Result.Container
        new TestDependency<_> (container, fun () -> container.DeleteContainerAsync().Wait())

    [<AssemblyCleanup>]
    static member Cleanup () =
        CosmosDbTest.db.DeleteAsync().Wait()
        CosmosDbTest.client.Dispose()
