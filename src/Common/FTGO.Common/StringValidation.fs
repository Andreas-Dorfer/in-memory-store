namespace FTGO.Common

module StringValidation =
    
    let minLength min str =
        if str |> String.length >= min then
            Some str
        else
            None
