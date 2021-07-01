namespace FTGO.Restaurant.InMemoryEntities

open System

[<CustomEquality; NoComparison>]
type internal Entry<'value> =
    {
        Version : uint64
        Deleted : bool
        Value : 'value
    }

    interface IEquatable<Entry<'value>> with
        member this.Equals other =
            if this.Deleted then
                false
            else
                this.Version = other.Version

    override this.Equals obj =
        match obj with
        | :? Entry<'value> as other ->
            (this :> IEquatable<_>).Equals other
        | _ -> false

    override this.GetHashCode () =
        HashCode.Combine (this.Deleted, this.Version)


module internal Entry =

    let create version value = {
        Version = version
        Deleted = false
        Value = value
    }

    let compare version = {
        Version = version
        Deleted = false
        Value = Unchecked.defaultof<_>
    }

    let delete () = {
        Version = Unchecked.defaultof<_>
        Deleted = true
        Value = Unchecked.defaultof<_>
    }
