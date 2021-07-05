namespace FTGO.Common.BaseTypes

#if DEBUG
type Undefined () = do invalidOp (nameof Undefined)
#endif
