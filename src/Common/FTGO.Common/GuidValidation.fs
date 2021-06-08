namespace FTGO.Common

open System

module GuidValidation =

    let notEmpty (guid : Guid) =
        if guid <> Guid.Empty then
            Some guid
        else
            None
