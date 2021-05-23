namespace FTGO.Common.BaseTypes

type ETag = ETag of string

type EntityId<'id> = EntityId of 'id * ETag
