namespace FTGO.Common.BaseTypes

type ETag = ETag of string

type Versioned<'a> = Versioned of 'a * ETag
    with member versioned.Value = let (Versioned (value, _)) = versioned in value
