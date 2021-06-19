namespace FTGO.Restaurant.Tests

open System

type TestDependency<'a> = TestDependency of 'a * IDisposable
    with
        member this.Value = let (TestDependency (value, _)) = this in value
        interface IDisposable with member this.Dispose () = let (TestDependency (_, disposable)) = this in disposable.Dispose ()