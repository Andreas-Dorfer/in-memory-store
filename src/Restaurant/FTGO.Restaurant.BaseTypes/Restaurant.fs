namespace FTGO.Restaurant.BaseTypes

type RestaurantId = private RestaurantId of int64

module RestaurantId =

    let create = RestaurantId
