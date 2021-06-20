namespace FTGO.Restaurant.Tests

open System

type TestDependency<'a> (value : 'a, dispose) =
    member _.Value = value
    interface IDisposable with member _.Dispose () = dispose ()
    new (value, disposable : IDisposable) = new TestDependency<_> (value, disposable.Dispose)
    new (value) = new TestDependency<_> (value, ignore)
