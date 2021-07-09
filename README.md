# in-memory-store
A thread-safe in-memory store with optimistic concurrency support. Great for testing / mocking and prototyping.
# AD.InMemoryStore
[![NuGet Package](https://img.shields.io/nuget/v/AndreasDorfer.InMemoryStore.svg)](https://www.nuget.org/packages/AndreasDorfer.InMemoryStore/)
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
    var newVersion = store.Update(key: 1, value: "B", version);
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
# AD.InMemoryStore.Functional
[![NuGet Package](https://img.shields.io/nuget/v/AndreasDorfer.InMemoryStore.Functional.svg)](https://www.nuget.org/packages/AndreasDorfer.InMemoryStore.Functional/)

A functional wrapper around `AD.InMemoryStore`. Optimized for F#.
