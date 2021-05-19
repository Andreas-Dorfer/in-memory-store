namespace FTGO.Common.Operators

open System.Diagnostics

[<AutoOpen>]
[<DebuggerStepThrough>]
module OptionOperators =

    /// <summary>Bind operator.</summary>
    let ( |>= ) a f =
        a |> Option.bind f

    /// <summary>Bind operator with reversed arguments.</summary>
    let ( =<| ) f a =
        a |>= f

    /// <summary>Map operator.</summary>
    let ( |>> ) a f =
        a |> Option.map f

    /// <summary>Map operator with reversed arguments.</summary>
    let ( <<| ) f a =
        a |>> f

    /// <summary>Bind composition operator.</summary>
    let ( >=> ) f g =
        f >> ((=<|) g)

    /// <summary>Bind compostion operator with reversed arguments.</summary>
    let ( <=< ) g f =
        f >=> g

    /// <summary>Map composition operator.</summary>
    let ( >|> ) f g =
        f >> ((<<|) g)

    /// <summary>Map composition operator with reversed arguments.</summary>
    let ( <|< ) g f =
        f >|> g
