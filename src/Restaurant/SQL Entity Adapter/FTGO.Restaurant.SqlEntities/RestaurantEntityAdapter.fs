namespace FTGO.Restaurant.SqlEntities

//open FTGO.Common.BaseTypes
//open FTGO.Restaurant.Entities
//open FTGO.Restaurant.BaseTypes

//module RestaurantEntityAdapter =
    
//    let create publishEvent : CreateRestaurantEntity =
//        fun createEvent ->
//            let createAndPublishEvent = createEvent >> publishEvent
//            fun args -> async {
//                let entity = {
//                    Id = EntityId (RestaurantId.create "" |> Option.get, ETag "")
//                    Name = args.Name
//                    Menu = []
//                }
//                do! entity |> createAndPublishEvent
//                return entity
//            }
