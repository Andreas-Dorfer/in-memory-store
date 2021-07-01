namespace FTGO.Restaurant.InMemoryEntities

open System.Collections.Concurrent

type InMemoryStore<'id, 'entity> = private InMemoryStore of ConcurrentDictionary<'id, Entry<'entity>>

module InMemoryStore =
    
    [<Literal>]
    let private insertVersion = 1uL

    let add (InMemoryStore store) (id, entity) =
        let entry = entity |> Entry.create insertVersion
        if store.TryAdd (id, entry) then
            Some (entry.Value, entry.Version)
        else
            None

    let get (InMemoryStore store) id =
        match store.TryGetValue id with
        | true, entry ->
            if entry.Deleted then
                None
            else
                Some (entry.Value, entry.Version)
        | _ -> None

    let getAll (InMemoryStore store) () =
         store.Values
         |> Seq.where (fun { Deleted = deleted } -> not deleted)
         |> Seq.map (fun { Version = version; Value = entity } -> (entity, version))
         |> List.ofSeq

    //let update (InMemoryStore store) (id, entity, version) =
    //    let inc version =
    //        let next = version + 1uL
    //        if next = errorVersion then
    //            insertVersion
    //        else
    //            next