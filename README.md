# AD.InMemoryStore
[![NuGet Package](https://img.shields.io/nuget/v/AndreasDorfer.InMemoryStore.svg)](https://www.nuget.org/packages/AndreasDorfer.InMemoryStore/)

A thread-safe in-memory store with optimistic concurrency support. Great for testing / mocking and prototyping.
## Namespace
```csharp
using AD.InMemoryStore;
```
## Create a Store
```csharp
InMemoryStore<int, string> store = new();
```
## Add a Value
```csharp
var version = store.Add(key: 1, value: "A");
```
## Get a Value
```csharp
try
{
    var (value, version) = store.Get(key: 1);
}
catch (KeyNotFoundException<int> ex)
{ }
```
## Get all Values
```csharp
foreach (var (key, value, version) in store.GetAll())
{ }
```
## Update a Value
```csharp
try
{
    var newVersion = store.Update(key: 1, value: "B", match: version);
}
catch (ConcurrencyException<int> ex)
{ }
```
## Update a Value with No Version Check
```csharp
try
{
    var newVersion = store.Update(key: 1, value: "B");
}
catch (KeyNotFoundException<int> ex)
{ }
```
## Remove a Value
```csharp
try
{
    store.Remove(key: 1, match: version);
}
catch (ConcurrencyException<int> ex)
{ }
```
## Remove a Value with No Version Check
```csharp
try
{
    store.Remove(key: 1);
}
catch (KeyNotFoundException<int> ex)
{ }
```
# AD.InMemoryStore.Functional
[![NuGet Package](https://img.shields.io/nuget/v/AndreasDorfer.InMemoryStore.Functional.svg)](https://www.nuget.org/packages/AndreasDorfer.InMemoryStore.Functional/)

A functional wrapper around `AD.InMemoryStore`. Optimized for F#.
## Namespace
```fsharp
open AD.InMemoryStore.Functional
```
## Create a Store
```fsharp
let store = InMemoryStore<int, string> ()
```
## Add a Value
```fsharp
match store.Add(key = 1, value = "A") with
| Ok (key, value, version) -> ()
| Error (AddError.DuplicateKey key) -> ()
```
