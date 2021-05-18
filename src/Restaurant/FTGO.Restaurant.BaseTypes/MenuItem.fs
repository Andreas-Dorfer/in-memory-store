namespace FTGO.Restaurant.BaseTypes

type MenuItemId = private MenuItemId of string

module MenuItemId =

    let create = MenuItemId
