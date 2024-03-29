[![NuGet Package](https://img.shields.io/nuget/v/AndreasDorfer.InMemoryStore.svg)](https://www.nuget.org/packages/AndreasDorfer.InMemoryStore/)
# AD.InMemoryStore
A thread-safe in-memory key-value store with optimistic concurrency support. Great for testing / mocking and prototyping.
## NuGet Package
    PM> Install-Package AndreasDorfer.InMemoryStore -Version 1.4.0
### Namespace
```csharp
using AD.InMemoryStore;
```
### Create a Store
```csharp
KeyValueStore<int, string> store = new();
```
### Add a Value
```csharp
try
{
    var version = store.Add(key: 1, value: "A");
}
catch (DuplicateKeyException<int> ex)
{ }
```
### Get a Value
```csharp
try
{
    var (value, version) = store.Get(key: 1);
}
catch (KeyNotFoundException<int> ex)
{ }
```
### Get all Values
```csharp
foreach (var (key, value, version) in store.GetAll())
{ }
```
### Update a Value
```csharp
try
{
    var newVersion = store.Update(key: 1, value: "B", match: version);
}
catch (VersionMismatchException<int> ex)
{ }
```
### Update a Value with No Version Check
```csharp
try
{
    var newVersion = store.Update(key: 1, value: "B", match: null);
}
catch (KeyNotFoundException<int> ex)
{ }
```
### Remove a Value
```csharp
try
{
    store.Remove(key: 1, match: version);
}
catch (VersionMismatchException<int> ex)
{ }
```
### Remove a Value with No Version Check
```csharp
try
{
    store.Remove(key: 1, match: null);
}
catch (KeyNotFoundException<int> ex)
{ }
```
---
[![NuGet Package](https://img.shields.io/nuget/v/AndreasDorfer.InMemoryStore.Functional.svg)](https://www.nuget.org/packages/AndreasDorfer.InMemoryStore.Functional/)
# AD.InMemoryStore.Functional
A functional wrapper around `AD.InMemoryStore`. Optimized for F#.
## NuGet Package
    PM> Install-Package AndreasDorfer.InMemoryStore.Functional -Version 1.4.0
### Namespace
```fsharp
open AD.InMemoryStore.Functional
```
### Create a Store
```fsharp
let store = KeyValueStore<int, string> ()
```
### Add a Value
```fsharp
match store.Add(key = 1, value = "A") with
| Ok (key, value, version) -> ()
| Error (AddError.DuplicateKey key) -> ()
```
Or if you don't care about the specific error type:
```fsharp
match store.Add(key = 1, value = "A") with
| Ok (key, value, version) -> ()
| Error (ErrorKey key) -> ()
```
### Get a Value
```fsharp
match store.Get(key = 1) with
| Ok (key, value, version) -> ()
| Error (GetError.KeyNotFound key) -> ()
```
Or if you don't care about the specific error type:
```fsharp
match store.Get(key = 1) with
| Ok (key, value, version) -> ()
| Error (ErrorKey key) -> ()
```
### Get all Values
```fsharp
for (key, value, version) in store.GetAll() do
    ()
```
### Update a Value
```fsharp
match store.Update(key = 1, value = "B", ``match`` = Some version) with
| Ok (key, value, version) -> ()
| Error error ->
    match error with
    | UpdateError.VersionMismatch key -> ()
    | _ -> ()
```
Or if you don't care about the specific error type:
```fsharp
match store.Update(key = 1, value = "B", ``match`` = Some version) with
| Ok (key, value, version) -> ()
| Error (ErrorKey key) -> ()
```
### Update a Value with No Version Check
```fsharp
match store.Update(key = 1, value = "B", ``match`` = None) with
| Ok (key, value, version) -> ()
| Error error ->
    match error with
    | UpdateError.KeyNotFound key -> ()
    | _ -> ()
```
Or if you don't care about the specific error type:
```fsharp
match store.Update(key = 1, value = "B", ``match`` = None) with
| Ok (key, value, version) -> ()
| Error (ErrorKey key) -> ()
```
### Remove a Value
```fsharp
match store.Remove(key = 1, ``match`` = Some version) with
| Ok key -> ()
| Error error ->
    match error with
    | RemoveError.VersionMismatch key -> ()
    | _ -> ()
```
Or if you don't care about the specific error type:
```fsharp
match store.Remove(key = 1, ``match`` = Some version) with
| Ok key -> ()
| Error (ErrorKey key) -> ()
```
### Remove a Value with No Version Check
```fsharp
match store.Remove(key = 1, ``match`` = None) with
| Ok key -> ()
| Error error ->
    match error with
    | RemoveError.KeyNotFound key -> ()
    | _ -> ()
```
Or if you don't care about the specific error type:
```fsharp
match store.Remove(key = 1, ``match`` = None) with
| Ok key -> ()
| Error (ErrorKey key) -> ()
```
