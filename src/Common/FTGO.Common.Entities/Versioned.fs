namespace FTGO.Common.Entities

open FTGO.Common.BaseTypes

type CreateEntity<'entity, 'event> = 'entity * 'event list -> Async<Versioned<'entity>>

type ReadEntity<'id, 'entity> = 'id -> Async<Versioned<'entity> option>
