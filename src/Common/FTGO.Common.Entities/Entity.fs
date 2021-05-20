namespace FTGO.Common.Entities

open FTGO.Common.BaseTypes

type IEntity<'id> =
    abstract member Id : EntityId<'id>
