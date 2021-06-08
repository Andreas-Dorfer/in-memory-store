namespace FTGO.Common.BaseTypes

type ETag = ETag of string

type Versioned<'a> = Versioned of 'a * ETag
