namespace FTGO.Common

module StringValidation =
    
    let minLength min value =
        if value |> String.length >= min then
            Some value
        else
            None
