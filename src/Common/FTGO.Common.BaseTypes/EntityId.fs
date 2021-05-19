namespace FTGO.Common.BaseTypes

type ETag = string

type EntityId<'id> = EntityId of 'id * ETag
