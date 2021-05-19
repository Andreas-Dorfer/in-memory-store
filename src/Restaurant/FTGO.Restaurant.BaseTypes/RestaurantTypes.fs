﻿namespace FTGO.Restaurant.BaseTypes

open FTGO.Common
open FTGO.Common.Operators

[<Struct>]
type RestaurantId = private RestaurantId of string

module RestaurantId =

    let create = StringValidation.minLength 1 >|> RestaurantId
